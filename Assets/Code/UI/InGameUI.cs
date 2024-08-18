using Code.Configs;
using Code.Gameplay;
using Code.Managers;
using Code.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class InGameUI : Singleton<InGameUI>, IRestart
    {
        private EnergyManager EnergyManager => EnergyManager.Instance;
        private BlocksManager BlocksManager => BlocksManager.Instance;
        private GameConfig GameConfig => GameConfig.Instance;
        
        [SerializeField] private Image _foundationEnergyBar;
        [SerializeField] private Image _foundationEnergyBarToSpend;
        [SerializeField] private Image _heightEnergyBar;
        [SerializeField] private Image _heightEnergyBarToSpend;
        [Space]
        [SerializeField] private Image _currentBlockImage;
        [SerializeField] private Image _nextBlockImage;

        private void Start()
        {
            EnergyManager.OnFoundationEnergyChanged += OnFoundationEnergyChanged;
            EnergyManager.OnHeightEnergyChanged += OnHeightEnergyChanged;
            BlocksManager.OnBlockTypeChanged += OnBlockTypeChanged;
            _foundationEnergyBar.fillAmount = 1;
            _heightEnergyBar.fillAmount = 1;
            _foundationEnergyBarToSpend.fillAmount = 0;
            _heightEnergyBarToSpend.fillAmount = 0;
            
            _currentBlockImage.color = GameConfig.BlockStats[BlocksManager.CurrentBlockType].Material.color;
            _nextBlockImage.color = GameConfig.BlockStats[BlocksManager.NextBlockType].Material.color;
        }
        
        private void OnHeightEnergyChanged(int energy)
        {
            float fillAmount = (float) energy / EnergyManager.GameConfig.MaxHeightEnergy;
            DOTween.Sequence()
                .Join(_heightEnergyBarToSpend.DOFillAmount(0, 0.2f))
                .Join(_heightEnergyBar.DOFillAmount(fillAmount, 0.2f));
        }

        private void OnFoundationEnergyChanged(int energy)
        {
            float fillAmount = (float) energy / EnergyManager.GameConfig.MaxFoundationEnergy;
            DOTween.Sequence()
                .Join(_foundationEnergyBarToSpend.DOFillAmount(0, 0.2f))
                .Join(_foundationEnergyBar.DOFillAmount(fillAmount, 0.2f));
        }

        public void SetFoundationEnergyToSpend(int energy)
        {
            float fillAmount = (float) energy / EnergyManager.GameConfig.MaxFoundationEnergy;
            _foundationEnergyBarToSpend.fillAmount = fillAmount;
        }
        
        public void SetHeightEnergyToSpend(int energy)
        {
            float fillAmount = (float) energy / EnergyManager.GameConfig.MaxHeightEnergy;
            _heightEnergyBarToSpend.fillAmount = fillAmount;
        }
        
        public void Restart()
        {
            _foundationEnergyBar.fillAmount = 1;
            _heightEnergyBar.fillAmount = 1;
            _foundationEnergyBarToSpend.fillAmount = 0;
            _heightEnergyBarToSpend.fillAmount = 0;
        }
        
        private void OnBlockTypeChanged(BlockType obj)
        {
            DOTween.Sequence()
                .Append(_currentBlockImage.rectTransform.DOAnchorPosY(100, 0.2f))
                .Join(_currentBlockImage.DOFade(0, 0.2f))
                .Join(_nextBlockImage.rectTransform.DOAnchorPosY(100, 0.2f))
                .Join(_nextBlockImage.DOFade(0, 0.2f))
                .AppendCallback(() =>
                {
                    _currentBlockImage.color = GameConfig.BlockStats[obj].Material.color;
                    _nextBlockImage.color = GameConfig.BlockStats[BlocksManager.NextBlockType].Material.color;
                })
                .Append(_currentBlockImage.rectTransform.DOAnchorPosY(-100, 0))
                .Join(_nextBlockImage.rectTransform.DOAnchorPosY(-100, 0))
                .Append(_currentBlockImage.rectTransform.DOAnchorPosY(0, 0.2f))
                .Join(_currentBlockImage.DOFade(1, 0.2f))
                .Join(_nextBlockImage.rectTransform.DOAnchorPosY(0, 0.2f))
                .Join(_nextBlockImage.DOFade(1, 0.2f));
        }
    }
}
