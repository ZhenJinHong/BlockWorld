using UnityEngine;

namespace VoxelWorld
{
    [RequireComponent(typeof(PlayerSync))]
    public class PlayerDefaultStatusRegister : MonoBehaviour
    {
        [SerializeField] PlayerGameInputProvider firstPersonInputValue = new PlayerGameInputProvider();
        PlayerSync playerSync;
        PlayerGameStatus playerGameStatus;
        private void Start()
        {
            playerSync = GetComponent<PlayerSync>();
            firstPersonInputValue.RegisterAction();
            playerGameStatus = new PlayerGameStatus(firstPersonInputValue, transform, Camera.main.transform);
            playerSync.playerGameStatus = playerGameStatus;
        }
        private void OnDestroy()
        {
            firstPersonInputValue.UnregisterAction();
        }
    }
}