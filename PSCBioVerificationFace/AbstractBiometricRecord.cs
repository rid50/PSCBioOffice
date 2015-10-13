using System;
using System.Windows.Forms;
using Neurotec.Biometrics;
using System.Drawing;
using Neurotec.Images;

namespace PSCBioVerificationFace
{
	public abstract class AbstractBiometricRecord : IDisposable
	{
		public abstract void AddToTemplate(NTemplate tmpl);

		public abstract object CreateView(Panel hostPanel);

		protected NImage _image;

		public NImage Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
			}
		}

		#region IDisposable Members

		public abstract void Dispose();

		#endregion
	}
}
