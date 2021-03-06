﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Movies.Core.Navigation;

namespace Movies.Core.Messaging
{
	public sealed class ShowViewMessage
	{
		public ShowViewMessage(ViewTargets viewTarget)
		{
			ViewTarget = viewTarget;
		}

		public ShowViewMessage(ViewTargets viewTarget, Object loadArgs)
			: this(viewTarget)
		{
			LoadArgs = loadArgs;
		}

		public ViewTargets ViewTarget { get; private set; }
		public Object LoadArgs { get; private set; }
	}
}
