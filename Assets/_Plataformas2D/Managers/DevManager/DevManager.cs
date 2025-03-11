using UnityEditor;
using UnityEngine;

public class DevManager : MonoBehaviourSingleton<DevManager>
{
#if UNITY_EDITOR
    [CustomEditor(typeof(DevManager)), CanEditMultipleObjects]
    public class DevManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //ZONA MODIFICABLE
            if (GUILayout.Button("GameOver"))
            {
                GameManager.Instance.GameOver();
            }

            if (GUILayout.Button("Win"))
            {
                GameManager.Instance.Win();
            }
            //FIN ZONA MODIFICIABLE
        }
    }
#endif
}
