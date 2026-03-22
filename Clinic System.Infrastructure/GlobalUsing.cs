global using Microsoft.AspNetCore.Identity;
global using Clinic_System.Core.Entities;
global using Clinic_System.Infrastructure.Identity;
global using Clinic_System.Application.Service.Interface;
global using Clinic_System.Core.Exceptions;
global using Hangfire;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.DependencyInjection;
global using Clinic_System.Application.Common; 
global using MailKit.Net.Smtp; 
global using MailKit.Security;
global using MimeKit;
global using Microsoft.Extensions.Options;
global using System.Linq.Expressions;
global using Clinic_System.Infrastructure.Helpers;
global using Clinic_System.Infrastructure.Services;
global using Clinic_System.Infrastructure.Services.Email;
global using Microsoft.Extensions.Configuration;
global using Clinic_System.Infrastructure.Authentication.Models;
global using Clinic_System.Infrastructure.Authentication;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using Clinic_System.Infrastructure.Authorization;
global using System.Security.Cryptography;
global using Clinic_System.Core.Interfaces.UnitOfWork;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.AspNetCore.Http;
global using System.Text.Json;
global using Microsoft.Extensions.Logging;
global using StackExchange.Redis; 
global using Google.Apis.Auth;












