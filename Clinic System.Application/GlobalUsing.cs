global using Clinic_System.Application.Service.Interface;
global using Clinic_System.Core.Entities;
global using MediatR;
global using Clinic_System.Application.Features.Doctors.Queries.Models;
global using Clinic_System.Core.Interfaces.UnitOfWork;
global using Clinic_System.Core.Enums;
global using Clinic_System.Application.DTOs.Doctors;
global using AutoMapper;
global using System.Net;
global using Clinic_System.Application.Common.Bases;
global using Clinic_System.Application.Common;
global using Clinic_System.Application.Features.Doctors.Commands.Models;
global using System.Transactions;
global using System.Text.Json.Serialization;
global using FluentValidation;
global using Clinic_System.Core.Exceptions;
global using Clinic_System.Application.DTOs;
global using Clinic_System.Application.DTOs.Appointments;
global using Clinic_System.Application.Features.Appointments.Queries.Models;
global using Clinic_System.Application.Features.Appointments.Commands.Models;
global using Microsoft.Extensions.Logging;
global using Clinic_System.Application.DTOs.Patients;
global using Clinic_System.Application.Features.Patients.Queries.Models;
global using Clinic_System.Application.Features.Patients.Commands.Models;
global using Clinic_System.Application.DTOs.Prescription;
global using Clinic_System.Application.DTOs.Payment;
global using System.Linq.Expressions;
global using Clinic_System.Application.Common.Behaviors;
global using Clinic_System.Application.Service.Implemention;
global using Microsoft.Extensions.DependencyInjection;
global using Clinic_System.Application.DTOs.MedicalRecord;
global using Clinic_System.Application.Features.MedicalRecords.Commands.Models;
global using Clinic_System.Application.Features.MedicalRecords.Queries.Models;
global using Clinic_System.Application.Features.Prescriptions.Commands.Models;
global using Clinic_System.Application.Features.Payment.Queries.Models;
global using Clinic_System.Application.Features.Payment.Commands.Models;
global using Clinic_System.Application.Features.Authentication.Commands.Models;
global using Clinic_System.Application.DTOs.Authentications;
global using Clinic_System.Application.Features.Authorization.Commands.Models;





