using UnityEngine;

public class StandUp : MonoBehaviour
{
    private Rigidbody2D m_Rid2D;
    private SpriteRenderer m_SR;

    private void Start()
    {
        m_Rid2D = transform.root.GetComponent<Rigidbody2D>();
        m_SR = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        transform.up = Vector2.up;
        var xMotion = m_Rid2D.velocity.x;
        if (xMotion > 0.01f) m_SR.flipX = false;
        else if (xMotion < 0.01f) m_SR.flipX = true;
    }
}
