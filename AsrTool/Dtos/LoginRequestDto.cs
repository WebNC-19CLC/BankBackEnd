﻿namespace AsrTool.Dtos
{
  public class LoginRequestDto
  {
    public string Username { get; set; }

    public string Password { get; set; }

    public string RecaptchaToken { get; set; }
  }
}