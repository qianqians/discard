using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class BushStomping : MonoBehaviour
{
  #region Inspector

  [SerializeField]
  private Sprite stompSprite = default;

  #endregion


  #region MonoBehaviour

  private void OnTriggerEnter2D (Collider2D other)
  {
    if ( other.CompareTag("Player") )
      GetComponent<SpriteRenderer>().sprite = stompSprite;
  }

  #endregion
}