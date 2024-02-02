using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Utils;
using Task = System.Threading.Tasks.Task;

namespace CardManagement.Physical
{
	/// <summary>
	/// Controls all the visuals of the <see cref="PhysicalCard"/>.
	/// </summary>
	public class PhysicalCardVisuals : MonoBehaviour
	{
		private const int ERROR_REMOVE_DELAY_MS = 2000;
		
		/// <summary>
		/// Error the card can show when tried to be played.
		/// </summary>
		public enum Error
		{
			NotEnoughMana,
			BoardIsFull,
			None
		}
        
		/// <summary>
		/// Side of the card.
		/// </summary>
		public enum Side
		{
			Front,
			Back,
		}
		
		/// <summary>
		/// Controls the cost visual.
		/// </summary>
		public IconController Cost => costRoot;

		/// <summary>
		/// Controls the health visual.
		/// </summary>
		public IconController Health => healthIcon;

		/// <summary>
		/// Controls the attack visual.
		/// </summary>
		public IconController Attack => attackIcon;

        [SerializeField] private Image image;
        [SerializeField] private IconController costRoot;
        [SerializeField] private IconController healthIcon;
        [SerializeField] private IconController attackIcon;
        [SerializeField] private GameObject frontObject;
        [SerializeField] private GameObject backObject;
        [SerializeField] private GameObject notEnoughManaError;
        [SerializeField] private GameObject boardIsFullError;
        
        private Error currentActiveError = Error.None;
        private Side cardSide = Side.Back;
        private CancellationTokenSource currentCancelSource = new CancellationTokenSource();
        
        public async void Initialize(AssetReference imageReference)
        {
	        image.sprite = await LoadSprite(imageReference);
        }
        
        /// <summary>
        /// Show an error on the card that disappears after a few seconds.
        /// </summary>
        /// <param name="targetError">Error relating to playing the card.</param>
        public void ShowError(Error targetError)
        {
	        if (currentActiveError != Error.None)
	        {
		        currentCancelSource.Cancel();
		        SetErrorActive(currentActiveError, false);
	        }

	        currentActiveError = targetError;
	        currentCancelSource = new CancellationTokenSource();
	        SetErrorActive(targetError, true);

	        _ = WaitToSetErrorInactive(targetError);
        }
        
        /// <summary>
        /// Flip the card visuals to show the back or the front depending on the current side.
        /// </summary>
        public void Flip()
        {
	        switch (cardSide)
	        {
		        case Side.Back:
			        cardSide = Side.Front;
			        frontObject.SetActive(true);
			        backObject.SetActive(false);
			        break;
		        case Side.Front:
			        cardSide = Side.Back;
			        frontObject.SetActive(false);
			        backObject.SetActive(true);
			        break;
	        }
        }
        
        private void SetErrorActive(Error targetError, bool setActive)
        {
	        switch (targetError)
	        {
		        case Error.NotEnoughMana or Error.None:
			        notEnoughManaError.SetActive(setActive);
			        break;
		        case Error.BoardIsFull or Error.None:
			        boardIsFullError.SetActive(setActive);
			        break;
	        }
        }
        
        private async Task<Sprite> LoadSprite(AssetReference imageReference)
        {
	        return await ResourceUtils.LoadAddressableFromReference<Sprite>(imageReference);
        }
        
        private async Task WaitToSetErrorInactive(Error error)
        {
	        await Task.Delay(ERROR_REMOVE_DELAY_MS, currentCancelSource.Token);
	        SetErrorActive(error, false);
        }
	}
}