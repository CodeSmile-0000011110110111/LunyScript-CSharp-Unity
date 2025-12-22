using Luny.Reflection;
using LunyEditor;
using LunyScript;
using System;
using System.Collections.Generic;

namespace LunyScript.Unity {} // placeholder

namespace LunyScriptEditor
{
	internal sealed class PreventCodeStrippingLinkerProcessor : LunyLinkerProcessor
	{
		private static List<PreserveDetails> PreserveAllLunyScriptTypes()
		{
			var details = new List<PreserveDetails>();

			var preserveScripts = new Dictionary<String, List<String>>();
			foreach (var scriptType in TypeDiscovery.FindAll<LunyScript.LunyScript>())
			{
				var assemblyName = scriptType.Assembly.GetName().Name;
				if (preserveScripts.ContainsKey(assemblyName) == false)
					preserveScripts[assemblyName] = new List<String>();

				preserveScripts[assemblyName].Add(scriptType.FullName);
			}

			foreach (var pair in preserveScripts)
				details.Add(new PreserveDetails { Assembly = pair.Key, Types = pair.Value.ToArray() });

			return details;
		}

		public override PreserveDetails[] GetPreserveDetails()
		{
			var details = PreserveAllLunyScriptTypes();

			details.Add(new PreserveDetails
			{
				Assembly = nameof(LunyScript),
				Types = new []
				{
					// script runner is discovered through reflection
					typeof(LunyScriptRunner).FullName,
				}
			});

			//details.Add(new PreserveDetails { Assembly = $"{nameof(LunyScript)}.{nameof(LunyScript.Unity)}" });

			return details.ToArray();
		}

		public override String GetLinkFilename() => nameof(LunyScript);
	}
}
