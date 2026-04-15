using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelCompletionPosition : LevelCondition
{
    [SerializeField] private float m_Radius;

    public override bool IsCompleted
    {
        get
        {
            if (Player.Instance.ActiveEnemy == null) return false;

            if(Vector3.Distance(Player.Instance.ActiveEnemy.transform.position, transform.position) <= m_Radius)
            {
                return true;
            }

            return false;
        }
    }

#if UNITY_EDITOR

    private static Color GizmoColor = new Color(0f, 1f, 0f, 0.3f);

    private void OnDrawGizmosSelected()
    {
        Handles.color = GizmoColor;
        Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
    }

#endif
}
