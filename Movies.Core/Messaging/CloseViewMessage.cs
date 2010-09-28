﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Core.Messaging
{
	public sealed class CloseViewMessage
	{
		public CloseViewMessage(String viewName)
		{
			ViewName = viewName;
		}

		public String ViewName { get; private set; }
	}
}
