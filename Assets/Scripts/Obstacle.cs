using System;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * 障碍物控制器
 */
public class Obstacle : MonoBehaviour
{
    public float maxSpinSpeed = 10f;// 障碍物最大旋转速度
    public float mixSize = 1f;//障碍物最小尺寸
    public float maxSize = 2.5f;//障碍物最大尺寸
    private float minxSpeed = 100f;// 障碍物最小速度
    private float maxSpeed = 250f;// 障碍物最大速度
    private Rigidbody2D rb;
    public GameObject bounceEffectPrefab;// 反弹特效预制件
    public float minImpactVelocity = 1f;  // 低于这个速度不生成特效
    public float scaleMultiplier = 0.1f;  // 冲击强度转缩放因子


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = Random.Range(mixSize, maxSize);// 障碍物随机大小
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        //获取Rigidbody2D组件并添加一个向右的初始力
        rb = GetComponent<Rigidbody2D>();
        //设置障碍物的随机速度 根据物体尺寸调整速度，使其更有重量
        float randomSpeed = Random.Range(minxSpeed, maxSpeed) / randomSize;
        //设置随机方向
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);
        float randomSpinSpeed = Random.Range(-maxSpinSpeed, maxSpinSpeed);// 障碍物随机旋转速度
        rb.AddTorque(randomSpinSpeed);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 获取对方刚体
        Rigidbody2D otherRb = collision.rigidbody;
        if (otherRb == null) return;

        // 当前物体速度 > 对方，才生成特效，防止两边都生成
        if (rb.linearVelocity.magnitude < otherRb.linearVelocity.magnitude) return;

        // 如果当前速度太小，不生成
        float impactSpeed = rb.linearVelocity.magnitude;
        if (impactSpeed < minImpactVelocity) return;

        // 获取碰撞点
        // 获取第一个接触点
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 point = contact.point;
        Vector2 normal = contact.normal;

        // 生成粒子特效
        GameObject fx = Instantiate(bounceEffectPrefab, point, Quaternion.identity);

        // 设置特效朝外喷发（基于法线方向）
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        fx.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 根据冲击速度动态缩放
        float scale = 1f + impactSpeed * scaleMultiplier;
        fx.transform.localScale = new Vector3(scale, scale, 1f);

        //Vector2 contactPoint = other.contacts[0].point; // 获取碰撞点
        //GameObject effect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);
        //Destroy(effect, 1f);
    }
}
