using Game2D.Gameplay;

using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 position;

    [SerializeField]
    private CharacterMovementController characterMovement;

    [SerializeField]
    private bool activete;

    #region Init

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (activete)
        {
            activete = false;
            SetDir();
        }
    }

#endif

    #endregion Init

    private void Awake()
    {
        characterMovement.OnDestinationReached += Print;
    }

    private float timeElapsed;

    private void SetDir()
    {
        timeElapsed = 0;
        characterMovement.MoveToPosition(position);
    }

    private void Print()
    {
        Debug.Log($"Time: {timeElapsed}");
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
    }
}