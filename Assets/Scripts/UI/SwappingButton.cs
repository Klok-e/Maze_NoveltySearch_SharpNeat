using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class SwappingButton : MonoBehaviour
    {
        [SerializeField] private string _firstText;
        [SerializeField] private string _secondText;
        [SerializeField] private UnityEvent _onFirstClick;
        [SerializeField] private UnityEvent _onSecondClick;

        private Button _button;
        private Text _text;

        private byte _state = 0;

        private void Start()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<Text>();

            _text.text = _firstText;

            _button.onClick.AddListener(ClickEvent);
        }

        private void ClickEvent()
        {
            if (_state % 2 == 0)
            {
                _text.text = _secondText;
                _onFirstClick.Invoke();
            }
            else
            {
                _text.text = _firstText;
                _onSecondClick.Invoke();
            }
            _state += 1;
        }
    }
}
