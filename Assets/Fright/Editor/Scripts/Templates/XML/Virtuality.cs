using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes how a function is virtualized
	public enum Virtuality
	{
		none,
		@abstract,
		@virtual,
		@override,
	}
}