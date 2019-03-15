using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;


public class ShapesManager : MonoBehaviour
{
    public Text DebugText, ScoreText;
    public bool ShowDebugInfo = false;

    public CompanionController Companion;

    public Canvas ShuffleCanvas;
    public Canvas WinCanvas;

    public SpriteRenderer Background;
    public GameObject Blackout;

    public ShapesArray shapes;
    public ShapesArray blocks;

    public GameObject BlockPrefab;

    private int score;
    public Slider ScoreSlider;

    private int counter;
    private int winNumber;

    public Vector2 SpriteSize;
    private Vector2 BottomRight;
    private Vector2 BottomRightBase;
    private Vector2 CandySize;
    private float Scale = 1f;
    private float FieldSize;

    private GameState state = GameState.None;
    private bool hasBomb = false;
    private GameObject hitGo = null;
    private Vector2[] SpawnPositions;
    public GameObject[] CandyPrefabs;
    public GameObject[] CustomShapes;
    private GameObject[] CurrentShapes;
    public GameObject[] ExplosionPrefabs;
    public GameObject[] BonusPrefabs;

    private System.Random rnd = new System.Random();

    public GameObject Bomb;

    private IEnumerator CheckPotentialMatchesCoroutine;
    private IEnumerator AnimatePotentialMatchesCoroutine;

    IEnumerable<GameObject> potentialMatches;

    public SoundManager soundManager;

    void Awake()
    {
        DebugText.enabled = ShowDebugInfo;
    }

    // Use this for initialization
    void Start()
    {
        ScoreSlider.maxValue = Constants.WinScore;

        FieldSize = gameObject.GetComponentInParent<SpriteRenderer>().bounds.size.x;
        BottomRightBase = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        InitializeTypesOnPrefabShapesAndBonuses();

        InitializeCandyAndSpawnPositions();
    }


    private void OnEnable()
    {
        //Background.gameObject.SetActive(true);

        if (Constants.CustomShapes)
        {
            CurrentShapes = CustomShapes;
        }
        else
        {
            CurrentShapes = CandyPrefabs;
        }

        float s = CurrentShapes[0].GetComponent<SpriteRenderer>().sprite.texture.height * 0.015f;
        SpriteSize.Set(s, s);

        if (FieldSize != 0) // To make sure this is not called on first Enable
        {
            InitializeTypesOnPrefabShapesAndBonuses();

            InitializeCandyAndSpawnPositions();
        }

    }

    private void OnDisable()
    {
        //Background.gameObject.SetActive(false);

        DestroyAllCandy();
        shapes = null;
        StopAllCoroutines();
    }
    /// <summary>
    /// Initialize shapes
    /// </summary>
    private void InitializeTypesOnPrefabShapesAndBonuses()
    {
        //just assign the name of the prefab
        foreach (var item in CurrentShapes)
        {
            item.GetComponent<Shape>().Type = item.name;

        }

        //assign the name of the respective "normal" candy as the type of the Bonus
        foreach (var item in BonusPrefabs)
        {
            item.GetComponent<Shape>().Type = CurrentShapes.
                Where(x => x.GetComponent<Shape>().Type.Contains(item.name.Split('_')[1].Trim())).Single().name;
        }
    }

    public void InitializeCandyAndSpawnPositionsFromPremadeLevel()
    {
        InitializeVariables();

        var premadeLevel = DebugUtilities.FillShapesArrayFromResourcesData();

        if (shapes != null)
            DestroyAllCandy();

        shapes = new ShapesArray();
        SpawnPositions = new Vector2[Constants.Columns];

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {

                GameObject newCandy = null;

                newCandy = GetSpecificCandyOrBonusForPremadeLevel(premadeLevel[row, column]);

                InstantiateAndPlaceNewCandy(row, column, newCandy);

            }
        }

        SetupSpawnPositions();
    }


    public void InitializeCandyAndSpawnPositions()
    {

        if (shapes != null)
            DestroyAllCandy();

        InitializeVariables();

        shapes = new ShapesArray();
        SpawnPositions = new Vector2[Constants.Columns];

        if(Constants.GameMode == 1)
            blocks = new ShapesArray();

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {

                GameObject newCandy = GetRandomCandy();

                //check if two previous horizontal are of the same type
                while (column >= 2 && shapes[row, column - 1].GetComponent<Shape>()
                    .IsSameType(newCandy.GetComponent<Shape>())
                    && shapes[row, column - 2].GetComponent<Shape>().IsSameType(newCandy.GetComponent<Shape>()))
                {
                    newCandy = GetRandomCandy();
                }

                //check if two previous vertical are of the same type
                while (row >= 2 && shapes[row - 1, column].GetComponent<Shape>()
                    .IsSameType(newCandy.GetComponent<Shape>())
                    && shapes[row - 2, column].GetComponent<Shape>().IsSameType(newCandy.GetComponent<Shape>()))
                {
                    newCandy = GetRandomCandy();
                }

                InstantiateAndPlaceNewCandy(row, column, newCandy);
                if (Constants.GameMode == 1)
                    InstantiateAndPlaceNewBlock(row, column);
            }
        }

        SetupSpawnPositions();

        StartCheckForPotentialMatches();
    }



    private void InstantiateAndPlaceNewCandy(int row, int column, GameObject newCandy)
    {
        GameObject go = Instantiate(newCandy,
            BottomRight + new Vector2(column * CandySize.x, row * CandySize.y), Quaternion.identity)
            as GameObject;

        go.transform.localScale = new Vector3(Scale, Scale);

        //assign the specific properties
        go.GetComponent<Shape>().Assign(newCandy.GetComponent<Shape>().Type, row, column);

        go.GetComponent<BoxCollider2D>().size = SpriteSize;

        shapes[row, column] = go;
    }

    private void InstantiateAndPlaceNewBlock(int row, int column)
    {
        GameObject go = Instantiate(BlockPrefab,
            BottomRight + new Vector2(column * CandySize.x, row * CandySize.y), Quaternion.identity)
            as GameObject;

        go.transform.localScale = new Vector3(Scale*1.5f, Scale*1.5f);

        //assign the specific properties
        go.GetComponent<Shape>().Assign("undamaged", row, column);

        blocks[row, column] = go;
    }

    private void SetupSpawnPositions()
    {
        //create the spawn positions for the new shapes (will pop from the 'ceiling')
        for (int column = 0; column < Constants.Columns; column++)
        {
            SpawnPositions[column] = BottomRight
                + new Vector2(column * CandySize.x, Constants.Rows * CandySize.y);
        }
    }




    /// <summary>
    /// Destroy all shape gameobjects
    /// </summary>
    private void DestroyAllCandy()
    {
        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                Destroy(shapes[row, column]);
                if (Constants.GameMode == 1)
                    Destroy(blocks[row, column]);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        DebugText.text = "State: " + state;

        if(Constants.TriggerStop)
        {           
            state = GameState.Drinking;
            Constants.TriggerStop = false;
        }

        if (Constants.TriggerGo)
        {
            state = GameState.None;
            StopCheckForPotentialMatches();
            hasBomb = true;
            Bomb.SetActive(true);
            //Companion.ToggleSpeech(true);
            Companion.StartSay("Wirf die Wasserbombe!", 5);
            Constants.TriggerGo = false;
        }

        if (ShowDebugInfo)
            DebugText.text = DebugUtilities.GetArrayContents(shapes);

        if (state == GameState.None)
        {
            //user has clicked or touched
            if (Input.GetMouseButtonDown(0))
            {
                //get the hit position
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null) //we have a hit!!!
                {
                    hitGo = hit.collider.gameObject;
                    if (!hasBomb)
                    {
                        state = GameState.SelectionStarted;
                    }
                    else
                    {
                        StartCoroutine(FindMatchesAndCollapse(true));
                        hasBomb = false;
                    }
                }
                
            }
        }
        else if (state == GameState.SelectionStarted)
        {
            //user dragged
            if (Input.GetMouseButton(0))
            {
                

                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                //we have a hit
                if (hit.collider != null && hitGo != hit.collider.gameObject)
                {

                    //user did a hit, no need to show him hints 
                    StopCheckForPotentialMatches();

                    //if the two shapes are diagonally aligned (different row and column), just return
                    if (!Utilities.AreVerticalOrHorizontalNeighbors(hitGo.GetComponent<Shape>(),
                        hit.collider.gameObject.GetComponent<Shape>()))
                    {
                        state = GameState.None;
                    }
                    else
                    {
                        state = GameState.Animating;
                        FixSortingLayer(hitGo, hit.collider.gameObject);
                        StartCoroutine(FindMatchesAndCollapse(hit));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Modifies sorting layers for better appearance when dragging/animating
    /// </summary>
    /// <param name="hitGo"></param>
    /// <param name="hitGo2"></param>
    private void FixSortingLayer(GameObject hitGo, GameObject hitGo2)
    {
        SpriteRenderer sp1 = hitGo.GetComponent<SpriteRenderer>();
        SpriteRenderer sp2 = hitGo2.GetComponent<SpriteRenderer>();
        if (sp1.sortingOrder <= sp2.sortingOrder)
        {
            sp1.sortingOrder = 1;
            sp2.sortingOrder = 0;
        }
    }



    /// <summary>
    /// Moves the shapes that the user wants to swap and check for matches
    /// </summary>
    /// <param name="hit2"></param>
    /// <returns></returns>
    private IEnumerator FindMatchesAndCollapse(RaycastHit2D hit2)
    {
        //get the second item that was part of the swipe
        var hitGo2 = hit2.collider.gameObject;
        shapes.Swap(hitGo, hitGo2);

        //move the swapped ones
        hitGo.transform.DOMove(hitGo2.transform.position, Constants.AnimationDuration);
        hitGo2.transform.DOMove(hitGo.transform.position, Constants.AnimationDuration);
        yield return new WaitForSeconds(Constants.AnimationDuration);

        //get the matches via the helper methods
        var hitGomatchesInfo = shapes.GetMatches(hitGo);
        var hitGo2matchesInfo = shapes.GetMatches(hitGo2);

        var totalMatches = hitGomatchesInfo.MatchedCandy
            .Union(hitGo2matchesInfo.MatchedCandy).Distinct();

        //if user's swap didn't create at least a 3-match, undo their swap
        if (totalMatches.Count() < Constants.MinimumMatches)
        {
            hitGo.transform.DOMove(hitGo2.transform.position, Constants.AnimationDuration);
            hitGo2.transform.DOMove(hitGo.transform.position, Constants.AnimationDuration);
            yield return new WaitForSeconds(Constants.AnimationDuration);

            shapes.UndoSwap();
        }

        //if more than 3 matches and no Bonus is contained in the line, we will award a new Bonus
        bool addBonus = totalMatches.Count() >= Constants.MinimumMatchesForBonus &&
            !BonusTypeUtilities.ContainsDestroyWholeRowColumn(hitGomatchesInfo.BonusesContained) &&
            !BonusTypeUtilities.ContainsDestroyWholeRowColumn(hitGo2matchesInfo.BonusesContained);

        Shape hitGoCache = null;
        GameObject sameTypeGo = null;
        if (addBonus)
        {
            //get the game object that was of the same type
            sameTypeGo = hitGomatchesInfo.MatchedCandy.Count() > 0 ? hitGo : hitGo2;
            hitGoCache = sameTypeGo.GetComponent<Shape>();
        }

        int timesRun = 1;
        while (totalMatches.Count() >= Constants.MinimumMatches)
        {
            //increase score
            IncreaseScore((totalMatches.Count() - 2) * Constants.Match3Score);

            if (timesRun >= 2)
                IncreaseScore(Constants.SubsequentMatchScore);

            //soundManager.PlayCrincle();

            foreach (var item in totalMatches)
            {
                shapes.Remove(item);
                RemoveFromScene(item);
            }

            //check and instantiate Bonus if needed
            if (addBonus)
                CreateBonus(sameTypeGo);

            addBonus = false;

            //get the columns that we had a collapse
            var columns = totalMatches.Select(go => go.GetComponent<Shape>().Column).Distinct();

            //the order the 2 methods below get called is important!!!
            //collapse the ones gone
            var collapsedCandyInfo = shapes.Collapse(columns);
            //create new ones
            var newCandyInfo = CreateNewCandyInSpecificColumns(columns);

            int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);

            //wait a bit after explosions
            yield return new WaitForSeconds(0.1f);

            MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);
            //MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);
            
            foreach(GameObject item in newCandyInfo.AlteredCandy)
            {
                MoveAndAnimateSingle(item, maxDistance);
                yield return new WaitForSeconds(0.05f);
            }



            //will wait for both of the above animations
            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration/* * maxDistance*/);

            //search if there are matches with the new/collapsed items
            totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy).
                Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();



            timesRun++;
        }

        CheckWinCondition();
    }

    /// <summary>
    /// Variant that is called either when a bomb is detonated to remove the surrounding objects (bomb = true)
    /// or after a shuffle to check for any matches on the whole field (bomb = false or no argument)
    /// </summary>
    /// <param name="bomb"></param>
    /// <returns></returns>
    private IEnumerator FindMatchesAndCollapse(bool bomb = false)
    {
        //get all matches via the helper method
        IEnumerable<GameObject> totalMatches;
        if (bomb)
        {
            var startPos = Bomb.transform.position;
            ThrowBomb(hitGo.transform.position);
            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration);
            ResetBomb(startPos);
            totalMatches = shapes.GetBombMatches(hitGo);
        }
        else
        {
            totalMatches = shapes.GetMatches();    
        }   

        int timesRun = 1;
        while (totalMatches.Count() >= Constants.MinimumMatches)
        {
            //increase score
            IncreaseScore((totalMatches.Count() - 2) * Constants.Match3Score);

            if (timesRun >= 2)
                IncreaseScore(Constants.SubsequentMatchScore);

            //soundManager.PlayCrincle();

            foreach (var item in totalMatches)
            {
                shapes.Remove(item);
                RemoveFromScene(item);
            }

            //get the columns that we had a collapse
            var columns = totalMatches.Select(go => go.GetComponent<Shape>().Column).Distinct();

            //the order the 2 methods below get called is important!!!
            //collapse the ones gone
            var collapsedCandyInfo = shapes.Collapse(columns);
            //create new ones
            var newCandyInfo = CreateNewCandyInSpecificColumns(columns);

            int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);

            //wait a bit after explosions
            yield return new WaitForSeconds(0.1f);

            MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);
            //MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);

            foreach (GameObject item in newCandyInfo.AlteredCandy)
            {
                MoveAndAnimateSingle(item, maxDistance);
                yield return new WaitForSeconds(0.05f);
            }



            //will wait for both of the above animations
            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration/* * maxDistance*/);

            //search if there are matches with the new/collapsed items
            totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy).
                Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();



            timesRun++;
        }

        CheckWinCondition();

    }

    private void DamageBlock(GameObject destroyed)
    {
        var s = destroyed.GetComponent<Shape>();
        int row = s.Row;
        int column = s.Column;

        var block = blocks[row, column];
        bool d = block.GetComponent<Shape>().Damage();

        if (!d)
            counter++;
    }

    private void CheckWinCondition()
    {
        if ((Constants.GameMode == 0 && score >= Constants.WinScore) || (Constants.GameMode == 1 && counter >= winNumber))
        {
            DestroyAllCandy();
            state = GameState.Paused;
            WinCanvas.gameObject.SetActive(true);
            Background.maskInteraction = SpriteMaskInteraction.None;
            Background.color = Color.white;
            Blackout.SetActive(true);
        }
        else
        {

            state = GameState.None;
            StartCheckForPotentialMatches();
        }
    }


    /// <summary>
    /// Creates a new Bonus based on the shape parameter
    /// </summary>
    /// <param name="hitGoCache"></param>
    //private void CreateBonus(Shape hitGoCache)
    //{
    //    GameObject Bonus = Instantiate(GetBonusFromType(hitGoCache.Type), BottomRight
    //        + new Vector2(hitGoCache.Column * CandySize.x,
    //            hitGoCache.Row * CandySize.y), Quaternion.identity)
    //        as GameObject;
    //    Bonus.transform.localScale *= Scale;
    //    Bonus.GetComponent<BoxCollider2D>().size = SpriteSize;

    //    shapes[hitGoCache.Row, hitGoCache.Column] = Bonus;
    //    var BonusShape = Bonus.GetComponent<Shape>();
    //    //will have the same type as the "normal" candy
    //    BonusShape.Assign(hitGoCache.Type, hitGoCache.Row, hitGoCache.Column);
    //    //add the proper Bonus type
    //    BonusShape.Bonus |= BonusType.DestroyWholeRowColumn;
    //}

    private void CreateBonus(GameObject hitGo)
    {
        var hitGoCache = hitGo.GetComponent<Shape>();

        GameObject Bonus = Instantiate(hitGo, BottomRight
            + new Vector2(hitGoCache.Column * CandySize.x,
                hitGoCache.Row * CandySize.y), Quaternion.identity)
            as GameObject;

        shapes[hitGoCache.Row, hitGoCache.Column] = Bonus;
        var BonusShape = Bonus.GetComponent<Shape>();
        //will have the same type as the "normal" candy
        BonusShape.Assign(hitGoCache.Type, hitGoCache.Row, hitGoCache.Column);
        //add the proper Bonus type
        BonusShape.Bonus |= BonusType.DestroyWholeRowColumn;

        Bonus.GetComponent<BoxCollider2D>().enabled = true;
        var glow = Bonus.transform.GetChild(1);
        glow.gameObject.SetActive(true);
        glow.GetComponent<GlowAnimator>().enabled = true;
    }


    /// <summary>
    /// Spawns new candy in columns that have missing ones
    /// </summary>
    /// <param name="columnsWithMissingCandy"></param>
    /// <returns>Info about new candies created</returns>
    private AlteredCandyInfo CreateNewCandyInSpecificColumns(IEnumerable<int> columnsWithMissingCandy)
    {
        AlteredCandyInfo newCandyInfo = new AlteredCandyInfo();

        //find how many null values the column has
        foreach (int column in columnsWithMissingCandy)
        {
            var emptyItems = shapes.GetEmptyItemsOnColumn(column);
            foreach (var item in emptyItems)
            {
                var go = GetRandomCandy();
                GameObject newCandy = Instantiate(go, SpawnPositions[column], Quaternion.identity)
                    as GameObject;
                newCandy.transform.localScale = new Vector2(Scale, Scale);
                newCandy.GetComponent<Shape>().Assign(go.GetComponent<Shape>().Type, item.Row, item.Column);
                newCandy.GetComponent<BoxCollider2D>().size = SpriteSize;

                if (Constants.Rows - item.Row > newCandyInfo.MaxDistance)
                    newCandyInfo.MaxDistance = Constants.Rows - item.Row;

                shapes[item.Row, item.Column] = newCandy;
                newCandyInfo.AddCandy(newCandy);
            }
        }
        return newCandyInfo;
    }

    /// <summary>
    /// Animates gameobjects to their new position
    /// </summary>
    /// <param name="movedGameObjects"></param>
    private void MoveAndAnimate(IEnumerable<GameObject> movedGameObjects, int distance)
    {
        foreach (var item in movedGameObjects)
        {
            item.transform.DOMove(BottomRight +
                new Vector2(item.GetComponent<Shape>().Column * CandySize.x, item.GetComponent<Shape>().Row * CandySize.y), Constants.MoveAnimationMinDuration /** distance*/);
            //item.transform.positionTo(Constants.MoveAnimationMinDuration * distance, BottomRight +
            //    new Vector2(item.GetComponent<Shape>().Column * CandySize.x, item.GetComponent<Shape>().Row * CandySize.y));
        }
    }

    /// <summary>
    /// Animates gameobjects to their new position
    /// </summary>
    /// <param name="movedGameObjects"></param>
    private void MoveAndAnimateSingle(GameObject movedGameObject, int distance)
    {
        movedGameObject.transform.DOMove(BottomRight +
                new Vector2(movedGameObject.GetComponent<Shape>().Column * CandySize.x, movedGameObject.GetComponent<Shape>().Row * CandySize.y), Constants.MoveAnimationMinDuration /** distance*/);
    }

    /// <summary>
    /// Destroys the item from the scene and instantiates a new explosion gameobject
    /// </summary>
    /// <param name="item"></param>
    private void RemoveFromScene(GameObject item)
    {
        GameObject explosion = GetRandomExplosion();
        var newExplosion = Instantiate(explosion, item.transform.position, Quaternion.identity) as GameObject;
        newExplosion.transform.localScale *= Scale;
        Destroy(newExplosion, Constants.ExplosionDuration);
        Destroy(item);

        if (Constants.GameMode == 1)
            DamageBlock(item);
    }

    /// <summary>
    /// Get a random candy
    /// </summary>
    /// <returns></returns>
    private GameObject GetRandomCandy()
    {
        return CurrentShapes[Random.Range(0, Constants.NumShapes)];
    }

    private void InitializeVariables()
    {
        Shuffle();       

        state = GameState.None;

        Background.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        Blackout.SetActive(false);

        if (Constants.GameMode == 0)
        {
            Background.color = new Vector4(Constants.MinColor, Constants.MinColor, Constants.MinColor, 1);
            ScoreSlider.gameObject.SetActive(true);
            score = 0;
            ShowScore();
        }
        else
        {
            Background.color = new Vector4(Constants.MaxColor, Constants.MaxColor, Constants.MaxColor, 1);
            ScoreSlider.gameObject.SetActive(false);
            counter = 0;
            winNumber = Constants.Rows * Constants.Columns * 2;
        }
            

        Scale = FieldSize / (SpriteSize.x * Constants.Rows);
        CandySize = SpriteSize * Scale;
        BottomRight = BottomRightBase + 0.5f * CandySize;
    }
    /// <summary>
    /// Increases the score and adjusts brightness of background image depending on the score
    /// </summary>
    /// <param name="amount"></param>
    private void IncreaseScore(int amount)
    {
        if (amount > Constants.Match3Score)
            Companion.SayGreat();

        if (Constants.GameMode == 0)
        {
            score += amount;
            ShowScore();
            float color = MapValue(score, 0, Constants.WinScore, Constants.MinColor, Constants.MaxColor);
            Background.color = new Vector4(color, color, color, 1);
        }

    }

    private void ShowScore()
    {
        ScoreText.text = "Score:\n" + score.ToString();
        ScoreSlider.value = score;
    }

    /// <summary>
    /// Get a random explosion
    /// </summary>
    /// <returns></returns>
    private GameObject GetRandomExplosion()
    {
        return ExplosionPrefabs[Random.Range(0, ExplosionPrefabs.Length)];
    }

    /// <summary>
    /// Gets the specified Bonus for the specific type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetBonusFromType(string type)
    {
        string color = type.Split('_')[1].Trim();
        foreach (var item in BonusPrefabs)
        {
            if (item.GetComponent<Shape>().Type.Contains(color))
                return item;
        }
        throw new System.Exception("Wrong type");
    }

    /// <summary>
    /// Starts the coroutines, keeping a reference to stop later
    /// </summary>
    private void StartCheckForPotentialMatches()
    {
        StopCheckForPotentialMatches();
        //get a reference to stop it later
        CheckPotentialMatchesCoroutine = CheckPotentialMatches();
        StartCoroutine(CheckPotentialMatchesCoroutine);
    }

    /// <summary>
    /// Stops the coroutines
    /// </summary>
    private void StopCheckForPotentialMatches()
    {
        if (AnimatePotentialMatchesCoroutine != null)
            StopCoroutine(AnimatePotentialMatchesCoroutine);
        if (CheckPotentialMatchesCoroutine != null)
            StopCoroutine(CheckPotentialMatchesCoroutine);
        ResetOpacityOnPotentialMatches();
    }

    /// <summary>
    /// Resets the opacity on potential matches (probably user dragged something?)
    /// </summary>
    private void ResetOpacityOnPotentialMatches()
    {
        if (potentialMatches != null)
            foreach (var item in potentialMatches)
            {
                if (item == null) break;

                Transform target = item.transform.GetChild(0);
                Color c = target.GetComponent<SpriteRenderer>().color;
                c.a = 0f;
                target.GetComponent<SpriteRenderer>().color = c;
                //Color c = item.GetComponent<SpriteRenderer>().color;
                //c.a = 1.0f;
                //item.GetComponent<SpriteRenderer>().color = c;
            }
    }

    /// <summary>
    /// Finds potential matches
    /// If none are found, Shuffles Game Field
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckPotentialMatches()
    {
        yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
        potentialMatches = Utilities.GetPotentialMatches(shapes);
        if (potentialMatches != null)
        {
            while (true)
            {

                AnimatePotentialMatchesCoroutine = Utilities.AnimatePotentialMatches(potentialMatches);
                StartCoroutine(AnimatePotentialMatchesCoroutine);
                yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
            }
        }
        else
        {
            ShuffleCanvas.gameObject.SetActive(true);
            state = GameState.Paused;
        }
    }

    /// <summary>
    /// Gets a specific candy or Bonus based on the premade level information.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private GameObject GetSpecificCandyOrBonusForPremadeLevel(string info)
    {
        var tokens = info.Split('_');

        if (tokens.Count() == 1)
        {
            foreach (var item in CurrentShapes)
            {
                if (item.GetComponent<Shape>().Type.Contains(tokens[0].Trim()))
                    return item;
            }

        }
        else if (tokens.Count() == 2 && tokens[1].Trim() == "B")
        {
            foreach (var item in BonusPrefabs)
            {
                if (item.name.Contains(tokens[0].Trim()))
                    return item;
            }
        }

        throw new System.Exception("Wrong type, check your premade level");
    }

    /// <summary>
    /// Shuffles the game field
    /// Called, if no more moves are possible
    /// </summary>
    public void ShuffleShapes()
    {
        ShuffleCanvas.gameObject.SetActive(false);

        shapes.Shuffle();

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                shapes[row, column].transform.position = BottomRight + new Vector2(column * CandySize.x, row * CandySize.y);
            }
        }

        StartCoroutine(FindMatchesAndCollapse());
    }

    public float MapValue(float a, float a0, float a1, float b0, float b1)
    {
        return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
    }

    private void ThrowBomb(Vector3 endPos)
    {
        Vector3 startPos = Bomb.transform.position;
        Bomb.transform.DOMove(endPos, Constants.MoveAnimationMinDuration);
    }

    private void ResetBomb(Vector3 startPos)
    {
        Bomb.transform.position = startPos;
        Bomb.SetActive(false);
        //Companion.ToggleSpeech(false);
    }

    /// <summary>
    /// Shuffle the array.
    /// </summary>
    /// <typeparam name="T">Array element type.</typeparam>
    /// <param name="array">Array to shuffle.</param>
    private void Shuffle()
    {
        int n = CurrentShapes.Length;
        for (int i = 0; i < n; i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + rnd.Next(n - i);
            var t = CurrentShapes[r];
            CurrentShapes[r] = CurrentShapes[i];
            CurrentShapes[i] = t;
        }
    }
}
