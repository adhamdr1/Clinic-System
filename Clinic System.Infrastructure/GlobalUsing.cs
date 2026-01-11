global using Microsoft.AspNetCore.Identity;
global using Clinic_System.Core.Entities;
global using Clinic_System.Infrastructure.Identity;
global using Clinic_System.Application.Service.Interface;
global using Clinic_System.Core.Exceptions;
global using Microsoft.EntityFrameworkCore;
global using Hangfire;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.DependencyInjection;
global using Clinic_System.Application.Common; // مكان كلاس الإعدادات
global using MailKit.Net.Smtp; // مهم جداً تستخدم دي مش System.Net.Mail
global using MailKit.Security;
global using MimeKit;
global using Microsoft.Extensions.Options;
global using System.Linq.Expressions;
global using Clinic_System.Infrastructure.Helpers;
global using Clinic_System.Infrastructure.Services;
global using Clinic_System.Infrastructure.Services.Email;
global using Microsoft.Extensions.Configuration;




