﻿using SFML.System;
using SFML.Window;

namespace agar.io.Input.Interfaces;

public interface IInput
{
	public Vector2f GetFinalPosition(Window window);
}