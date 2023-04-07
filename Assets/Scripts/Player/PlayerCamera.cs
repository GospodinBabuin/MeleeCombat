using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject _cinemachineCameraTarget;
    private PlayerInputActions _inputActions;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private readonly float _threshold = 0.01f;
    private readonly float _topClamp = 70.0f;
    private readonly float _bottomClamp = -30.0f;

    private void Start()
    {
        _inputActions = GetComponent<PlayerInputActions>();
        _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (_inputActions.Look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _inputActions.Look.x;
            _cinemachineTargetPitch += _inputActions.Look.y;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
