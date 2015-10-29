using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstanListViewControler : MonoBehaviour {

    // events to listen to if needed...
    public delegate void InfiniteItemIsPressed (int itemDataIndex, bool isDown);

    public event InfiniteItemIsPressed InfiniteItemIsPressedEvent;
    public delegate void InfiniteItemIsClicked (int itemDataIndex);

    public event InfiniteItemIsClicked InfiniteItemIsClickedEvent;

    //Prefabs
    public Transform itemPrefab;
    // NGUI Controllers
    public UITable table;
    public UIScrollView draggablePanel;
    //scroll indicator
    private int scrollCursor = 0;
    // pool
    public float cellHeight = 220f;// at the moment we support fixed height... insert here or measure it
    private int poolSize = 6;
    private List<Transform> itemsPool = new List<Transform> ();
    private int extraBuffer = 8;
    private int startIndex = 0; // where to start
    private Hashtable dataTracker = new Hashtable ();// hashtable to keep track of what is being displayed by the pool

    //our data...using arraylist for generic types... if you want specific types just refactor it to List<T> where T is the type
    // Dối tượng trong list tùy biến theo từng project
    private List<TableBehavior> originalData = new List<TableBehavior> ();
    private List<TableBehavior> dataList = new List<TableBehavior> ();

    void Start () {
        //table.Reposition ();
        //draggablePanel.ResetPosition ();
    }
    // Update is called once per frame
    void Update () {

    }

    #region Infinite List Data Sources calls

    void cloneItem (Transform item, int i) {
        TableBehavior tableBehavior = item.GetComponent<TableBehavior> ();
        tableBehavior.id = dataList[i].id;
        tableBehavior.status = dataList[i].status;
        tableBehavior.name = dataList[i].name;
        tableBehavior.masid = dataList[i].masid;
        tableBehavior.nUser = dataList[i].nUser;
        tableBehavior.maxUser = dataList[i].maxUser;
        tableBehavior.money = dataList[i].money;
        tableBehavior.needMoney = dataList[i].needMoney;
        tableBehavior.maxMoney = dataList[i].maxMoney;
        tableBehavior.Lock = dataList[i].Lock;
        tableBehavior.typeTable = dataList[i].typeTable;
        tableBehavior.choinhanh = dataList[i].choinhanh;
    }

    /*Đổ dữ liệu vào item*/
    void PopulateListItemWithIndex (Transform item, int dataIndex) {
        cloneItem (item, dataIndex);
        item.GetComponent<TableBehavior> ().setInFo (dataIndex);
    }


    #endregion

    #region Infinite List Management & scrolling
    // set then call InitTableView
    public void SetStartIndex (int inStartIndex) {
        startIndex = inStartIndex;
    }

    public void SetOriginalData (List<TableBehavior> inDataList) {
        originalData = new List<TableBehavior> (inDataList);
    }
    // call to refresh without changing sections.. e.g. jump to specific point...
    public void RefreshTableView () {

        if(originalData == null || originalData.Count == 0)
            Debug.LogWarning ("InfiniteListPopulator.InitTableView() trying to refresh with no data");

        InitTableView (originalData, startIndex);
    }

    public void InitTableView (List<TableBehavior> inDataList, int inStartIndex) {
        InitTableViewImp (inDataList, inStartIndex);

    }
    #endregion

    #region The private stuff... ideally you shouldn't need to call or change things directly from this region onwards
    void InitTableViewImp (List<TableBehavior> inDataList, int inStartIndex) {
        RefreshPool ();
        startIndex = inStartIndex;
        scrollCursor = inStartIndex;
        dataTracker.Clear ();
        originalData.Clear ();
        dataList.Clear ();
        originalData = new List<TableBehavior> (inDataList);
        dataList = new List<TableBehavior> (inDataList);

        int j = 0;
        for(int i=startIndex; i < dataList.Count; i++) {
            Transform item = GetItemFromPool (j);
            if(item != null) {
                InitListItemWithIndex (item, i, j);
                j++;
            } else {
                break;
            }
        }

        // at the moment we are repositioning the list after a delay... repositioning immediatly messes up the table when refreshing... no clue why...
        Invoke ("RepositionList", 0.1f);
    }

    void RepositionList () {
        table.Reposition ();
        // make sure we have a correct poistion sequence
        draggablePanel.SetDragAmount (0, 0, false);

        for(int i = 0; i < itemsPool.Count; i++) {
            Transform item = itemsPool[i];
            item.localPosition = new Vector3 (item.localPosition.x, -((cellHeight / 2) + i * cellHeight), item.localPosition.z);
        }

    }


    // items
    void InitListItemWithIndex (Transform item, int dataIndex, int poolIndex) {
        item.GetComponent<TableBehavior> ().itemDataIndex = dataIndex;
        item.GetComponent<TableBehavior> ().listPopulator = this;
        item.GetComponent<TableBehavior> ().panel = draggablePanel.panel;
        item.name = "item" + dataIndex;
        PopulateListItemWithIndex (item, dataIndex);
        dataTracker.Add (itemsPool[poolIndex].GetComponent<TableBehavior> ().itemDataIndex, itemsPool[poolIndex].GetComponent<TableBehavior> ().itemNumber);

    }

    void PrepareListItemWithIndex (Transform item, int newIndex, int oldIndex) {
        if(newIndex < oldIndex)
            item.localPosition += new Vector3 (0, (poolSize) * cellHeight, 0);
        else
            item.localPosition -= new Vector3 (0, (poolSize) * cellHeight, 0);

        item.GetComponent<TableBehavior> ().itemDataIndex = newIndex;
        item.name = "item" + (newIndex);

        PopulateListItemWithIndex (item, newIndex);
        dataTracker.Add (newIndex, (int) (dataTracker[oldIndex]));
        dataTracker.Remove (oldIndex);
    }

    // the main logic for "infinite scrolling"...
    private bool isUpdatingList = false;

    public IEnumerator ItemIsInvisible (int itemNumber) {
        if(isUpdatingList)
            yield return null;
        isUpdatingList = true;
        if(dataList.Count > poolSize) {// we need to do something "smart"...
            Transform item = itemsPool[itemNumber];
            int itemDataIndex = 0;
            if(item.tag.Equals (itemPrefab.gameObject.tag.ToString ()))
                itemDataIndex = item.GetComponent<TableBehavior> ().itemDataIndex;

            int indexToCheck = 0;
            TableBehavior infItem = null;
            TableBehavior infSection = null;
            if(dataTracker.ContainsKey (itemDataIndex + 1)) {
                infItem = itemsPool[(int) (dataTracker[itemDataIndex + 1])].GetComponent<TableBehavior> ();
                infSection = itemsPool[(int) (dataTracker[itemDataIndex + 1])].GetComponent<TableBehavior> ();

                if((infItem != null && infItem.verifyVisibility ()) || (infSection != null && infSection.verifyVisibility ())) {
                    // dragging upwards (scrolling down)
                    indexToCheck = itemDataIndex - (extraBuffer / 2);
                    if(dataTracker.ContainsKey (indexToCheck)) {
                        //do we have an extra item(s) as well?
                        for(int i = indexToCheck; i >= 0; i--) {
                            if(dataTracker.ContainsKey (i)) {
                                infItem = itemsPool[(int) (dataTracker[i])].GetComponent<TableBehavior> ();
                                infSection = itemsPool[(int) (dataTracker[i])].GetComponent<TableBehavior> ();
                                if((infItem != null && !infItem.verifyVisibility ()) || (infSection != null && !infSection.verifyVisibility ())) {
                                    item = itemsPool[(int) (dataTracker[i])];
                                    if((i) + poolSize < dataList.Count && i > -1) {

                                        PrepareListItemWithIndex (item, i + poolSize, i);
                                    }
                                }
                            } else {
                                scrollCursor = itemDataIndex - 1;
                                break;
                            }
                        }
                    }
                }
            }
            if(dataTracker.ContainsKey (itemDataIndex - 1)) {
                infItem = itemsPool[(int) (dataTracker[itemDataIndex - 1])].GetComponent<TableBehavior> ();
                infSection = itemsPool[(int) (dataTracker[itemDataIndex - 1])].GetComponent<TableBehavior> ();

                if((infItem != null && infItem.verifyVisibility ()) || (infSection != null && infSection.verifyVisibility ())) {
                    //dragging downwards check the item below
                    indexToCheck = itemDataIndex + (extraBuffer / 2);

                    if(dataTracker.ContainsKey (indexToCheck)) {
                        // if we have an extra item
                        for(int i = indexToCheck; i < dataList.Count; i++) {
                            if(dataTracker.ContainsKey (i)) {
                                infItem = itemsPool[(int) (dataTracker[i])].GetComponent<TableBehavior> ();
                                infSection = itemsPool[(int) (dataTracker[i])].GetComponent<TableBehavior> ();
                                if((infItem != null && !infItem.verifyVisibility ()) || (infSection != null && !infSection.verifyVisibility ())) {
                                    item = itemsPool[(int) (dataTracker[i])];
                                    if((i) - poolSize > -1 && (i) < dataList.Count) {
                                        PrepareListItemWithIndex (item, i - poolSize, i);
                                    }
                                }
                            } else {
                                scrollCursor = itemDataIndex + 1;
                                break;
                            }
                        }
                    }
                }
            }
        }
        isUpdatingList = false;
    }

    #endregion
    #region items callbacks and helpers
    public void itemIsPressed (int itemDataIndex, bool isDown) {
        if(InfiniteItemIsPressedEvent != null)
            InfiniteItemIsPressedEvent (itemDataIndex, isDown);
    }

    public void itemClicked (int itemDataIndex) {
        if(InfiniteItemIsClickedEvent != null)
            InfiniteItemIsClickedEvent (itemDataIndex);
    }
    #endregion
    #region Pool & sections Management

    Transform GetItemFromPool (int i) {
        if(i >= 0 && i < poolSize) {
            itemsPool[i].gameObject.SetActive (true);
            return itemsPool[i];
        } else
            return null;
    }

    void RefreshPool () {
        poolSize = (int) (draggablePanel.panel.baseClipRegion.w / cellHeight) + extraBuffer;

        // destroy current items
        for(int i=0; i < itemsPool.Count; i++) {
            Object.Destroy (itemsPool[i].gameObject);
        }
        itemsPool.Clear ();
        for(int i=0; i < poolSize; i++) { // the pool will use itemPrefab as a default
            Transform item = Instantiate (itemPrefab) as Transform;
			//item.transform.localPosition = new Vector3(0,0,0);
            item.gameObject.SetActive (false);
            item.GetComponent<TableBehavior> ().itemNumber = i;
            item.name = "item" + i;
            item.parent = table.transform;
            itemsPool.Add (item);
        }

    }
    #endregion
}
