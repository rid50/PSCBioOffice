using System;
using System.Collections.Generic;
using System.Text;

namespace Neurotec.Samples
{
	class SampleSettingsProvider : System.Configuration.LocalFileSettingsProvider
	{
		private static bool _monoRuntime = false;
		static SampleSettingsProvider()
		{
			Type t = Type.GetType("Mono.Runtime");
			if (t != null)
				_monoRuntime = true;
			else
				_monoRuntime = false;
		}
		
		
		public static string ProviderName
		{
			get {return "SampleSettingsProvider";}
		}
		
		public static bool IsMonoRuntime
		{
			get 
			{
				return _monoRuntime;
			}
		}
		
		private MonoFileSettingsProvider monoImpl = null;
		
		public override void Initialize (string name, System.Collections.Specialized.NameValueCollection values)
		{
			if (name != null)
				base.Initialize (name, values);
			else
				base.Initialize(ProviderName, values);
			if (IsMonoRuntime)
			{
				monoImpl = new MonoFileSettingsProvider();
			}
		}

		

		public override System.Configuration.SettingsPropertyValueCollection GetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyCollection collection)
		{
			if (_monoRuntime)
			{
				return monoImpl.GetPropertyValues(context, collection);
			}
			else
			{
				return base.GetPropertyValues(context, collection);
			}
		}

		public override void SetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyValueCollection collection)
		{
			if (_monoRuntime)
			{
				monoImpl.SetPropertyValues(context, collection);
			}
			else
			{
				base.SetPropertyValues(context, collection);
			}
		}
		
	}
}
