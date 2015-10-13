using Neurotec.Devices;
using System.ComponentModel;

namespace PSCBioVerificationFace.Common
{
	public delegate void OnDeviceChange(object sender, NDeviceManagerDeviceEventArgs e);

	public class Devices
	{
		#region Private fields
		private static Devices _instance = null;
		private NDeviceManager _fingerScanners = null;
		private NDeviceManager _irisScanners = null;
		private NDeviceManager _cameras = null;
		private NDeviceManager _palmScanners = null;
		private NDeviceManager _microphones = null;
		#endregion

		#region Public static methods

		public static Devices Instance
		{
			get
			{
				if (_instance == null)
					_instance = new Devices();
				return _instance;
			}
		}

		public NDeviceManager FingerScanners
		{
			get
			{
				return _fingerScanners;
			}
		}
		public NDeviceManager PalmScanners
		{
			get
			{
				return _palmScanners;
			}
		}
		public NDeviceManager Cameras
		{
			get
			{
				return _cameras;
			}
		}
		public NDeviceManager IrisScanners
		{
			get
			{
				return _irisScanners;
			}
		}
		public NDeviceManager Microphones
		{
			get
			{
				return _microphones;
			}
		}
		private Devices()
		{
			_fingerScanners = new NDeviceManager(NDeviceType.FScanner);
			_palmScanners = new NDeviceManager(NDeviceType.PalmScanner);
			_microphones = new NDeviceManager(NDeviceType.Microphone);
			_cameras = new NDeviceManager(NDeviceType.Camera);
			_irisScanners = new NDeviceManager(NDeviceType.IrisScanner);
		}

		#endregion
	}
}
