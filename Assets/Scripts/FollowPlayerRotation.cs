using UnityEngine;

public class FollowPlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void Update()
    {
        if (_player != null)
        {
            Vector3 direction = _player.position - transform.position;

            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}