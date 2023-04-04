using UnityEngine.SceneManagement;

namespace AiTest.Units.Enemies
{
    public class ImmediateAttack
    {
        public UnitType TargetType { get; private set; }

        public ImmediateAttack(UnitType targetType) 
        {
            TargetType = targetType;
        }

        public void TryDestroy(Unit unit)
        {
            UnitType type = unit.Type;

            if (type != TargetType)
                return;

            UnityEngine.Object.Destroy(unit.gameObject);

            // Bad code. Only for restart of scene
            if (type == UnitType.Player)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}
