using UnityEngine;
using UnityEngine.UI;

public class ball : MonoBehaviour {

    public GameObject bs, brick;
    public Button begin, left, right, up, down;
    public Text win;

    private bool state = false;
    private int remain = 12, w = 0;
    private GameObject[,,] bricks = new GameObject[2, 3, 2];

    void Start () {
        begin.onClick.AddListener(BeginOnClick);
        up.onClick.AddListener(delegate { ArrowOnClick("up"); });
        down.onClick.AddListener(delegate { ArrowOnClick("down"); });
        left.onClick.AddListener(delegate { ArrowOnClick("left"); });
        right.onClick.AddListener(delegate { ArrowOnClick("right"); });
        for(int x = 0; x < 2; x++)
            for(int y = 0; y < 3; y++)
                for(int z = 0; z < 2; z++) {
                    bricks[x, y, z] = Instantiate(brick);
                    bricks[x, y, z].transform.position = new Vector3(-2 + 4 * x, 9.25f + y, -2 + 4 * z);
                    bricks[x, y, z].name = "brick";
                }
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}

    private void restart() {
        begin.gameObject.SetActive(true);
        transform.position = new Vector3(0, 1.75f, 0);
        bs.transform.position = new Vector3(0, 1, 0);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        remain = 12;
        state = false;
        for(int x = 0; x < 2; x++)
            for(int y = 0; y < 3; y++)
                for(int z = 0; z < 2; z++) {
                    bricks[x, y, z].SetActive(true);
                }
    }

    private void ArrowOnClick(string key) {
        float dx = 0, dz = 0, bx = bs.transform.position.x, bz = bs.transform.position.z;
        switch (key) {
            case "left":
                dx = 1;
                break;
            case "right":
                dx = -1;
                break;
            case "up":
                dz = -1;
                break;
            case "down":
                dz = 1;
                break;
        }
        if (bx + dx > 3 || bz + dz > 3 || bx + dx < -3 || bz + dz < -3) return;
        if (! state) {
            transform.Translate(dx, 0, dz);
            bs.transform.Translate(dx, 0, dz);
        }
        else {
            bs.transform.Translate(dx, 0, dz);
        }
    }

    private void BeginOnClick() {
        Vector3 v = Random.onUnitSphere;
        if (v.y < 0) v *= (-5); else v *= 5;
        GetComponent<Rigidbody>().velocity = v;
        begin.gameObject.SetActive(false);
        state = true;
    }
    
    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "bottom") {
            win.text = "已连胜0局"; w = 0;
            restart();
        }
        if (collision.gameObject.name == "brick") {
            collision.gameObject.SetActive(false);
            remain--;
            Debug.Log(remain);
            if(remain == 0) {
                w++;
                win.text = "已连胜" + w + "局";
                restart();
            }
        }
    }

}
