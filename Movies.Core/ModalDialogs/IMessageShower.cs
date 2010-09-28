﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Movies.Core.ModalDialogs
{
	public interface IMessageShower
	{
		MessageBoxResult Show(String message, String caption, MessageBoxButton button);
	}
}
