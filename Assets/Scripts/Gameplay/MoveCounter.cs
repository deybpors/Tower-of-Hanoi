using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _movesText;
    public UIAnimation uiAnimation;
    private Animator _animator;
    private int _moves;
    void Start()
    {
        Manager.instance.moveCounter = this;
        _movesText.text = _moves.ToString();
    }

    void OnEnable()
    {
        if (_animator == null)
        {
            _animator = _movesText.GetComponent<Animator>();
        }
        _animator.Play("Normal");
    }

    void OnDisable()
    {
        _animator.Play("Normal");
    }

    public int AddMoves(int add)
    {
        _moves += add;
        _movesText.text = _moves.ToString();
        _animator.Play("Add");
        return _moves;
    }

    public void ResetMoves()
    {
        _moves = 0;
        _movesText.text = _moves.ToString();
        _animator.Play("Normal");
    }
}
