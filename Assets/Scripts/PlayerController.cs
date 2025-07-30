using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/**
 * 飞船控制器
 */
public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;// 飞船存活的时间
    private int score = 0;// 飞船存活得分
    private float scoreMultiplier = 10f;// 飞船存活得分倍率
    public float thrustForce = 3f;// 飞船推进力
    public float maxSpeed = 5f;// 飞船最大速度
    public GameObject boosterFlame;// 飞船推进器火焰特效
    private Rigidbody2D rb;// 飞船的刚体组件
    public UIDocument uiDocument; // UI 文档引用
    private Label scoreLabel; // 用于显示分数的 UI 标签
    private Label highScoreLabel; // 用于显示分数的 UI 标签
    private Button restartBotton;// 用于显示重启提示的按钮
    public GameObject explosionPrefab; // 爆炸特效预制件
    private int highScore = 0; // 最高分数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreLabel"); // 获取 UI 文档中的分数标签
        restartBotton = uiDocument.rootVisualElement.Q<Button>("RestartButton"); // 获取 UI 文档中的重启提示标签
        restartBotton.style.display = DisplayStyle.None; // 初始时隐藏重启提示按钮
        restartBotton.clicked += ReloadScene; // 添加按钮点击事件监听器
        highScoreLabel = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel"); // 获取 UI 文档中的最高分标签
        highScore = PlayerPrefs.GetInt("HighScore", 0); // 获取存储的最高分数
        //StopAudio();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        Controller();
    }

    //当一个 2D 碰撞器撞击另一个 2D 碰撞器时，Unity 会自动调用此方法
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        // 实例化爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        restartBotton.style.display = DisplayStyle.Flex; // 显示重启提示按钮
        if (highScore < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score; // 更新最高分数
        }
        highScoreLabel.style.display = DisplayStyle.Flex;
        highScoreLabel.text = $"High Score: {highScore}"; // 显示最高分数
        //PlayAudio();
    }

    /**
     * 更新分数
     */
    private void UpdateScore()
    {
        elapsedTime += Time.deltaTime; // 更新飞船存活时间
        //Mathf.FloorToInt()取小数（浮点数）并返回小于或等于该值的最大整数。不是四舍五入
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier); //计算飞船存活得分
        scoreLabel.text = $"score：{score}";//记录存活的分数并显示在 UI 上
    }

    /**
     *  控制飞船
     */
    private void Controller()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            //鼠标左键是否按下 mousePos表示鼠标在世界坐标系中的位置
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

            //normalized是一个向量属性，它将向量的长度调整为 1（但保持其方向）。
            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction; // 设置飞船朝向鼠标位置

            // 计算飞船的推进方向 normalized 确保了无论距离多远，所有的点击都会施加相同大小的推力。
            rb.AddForce(direction * thrustForce);
        }

        // 鼠标左键按下时激活推进器火焰特效，松开时停止
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true); // 激活推进器火焰特效
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);// 停止推进器火焰特效
        }

        // 限制飞船的速度
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    /**
     *  重启游戏
     */
    private void ReloadScene()
    {
        //通过场景管理器重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);// 重载当前场景
    }

}
