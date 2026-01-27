namespace Clinic_System.Application.Common
{
    public static class EmailTemplates
    {
        public static string GetBookingConfirmation(string patientName, string doctorName, string specialty, DateTime date)
        {
            return $@"
            <div style='font-family: Arial, sans-serif; direction: ltr; color: #333;'>
                <h2 style='color: #2c3e50;'>Appointment Confirmation</h2>
                <p>Dear <strong>{patientName}</strong>,</p>
                <p>Your booking has been successfully confirmed with the following details:</p>
                <div style='background-color: #f8f9fa; padding: 15px; border-left: 5px solid #007bff;'>
                    <p><strong>Doctor:</strong> Dr. {doctorName}</p>
                    <p><strong>Specialty:</strong> {specialty}</p>
                    <p><strong>Date:</strong> {date:dddd, dd MMMM yyyy}</p>
                    <p><strong>Time:</strong> {date:hh:mm tt}</p>
                </div>
                <p>We look forward to seeing you at the clinic.</p>
                <br>
                <p>Best Regards,<br><strong>Elite Clinic Team</strong></p>
            </div>";
        }

        public static string GetReschedulingConfirmation(string patientName, string doctorName, string specialty, DateTime oldDate, DateTime newDate)
        {
            return $@"
                 <div style='font-family: Arial, sans-serif; direction: ltr; color: #333; line-height: 1.6; max-width: 600px; border: 1px solid #eee; padding: 20px;'>
                     <h2 style='color: #e67e22; border-bottom: 2px solid #f39c12; padding-bottom: 10px;'>Appointment Rescheduled</h2>
                     <p>Dear <strong>{patientName}</strong>,</p>
                     <p>Please be informed that your appointment with <strong>Dr. {doctorName}</strong> has been successfully rescheduled.</p>
                     
                     <div style='margin: 20px 0;'>
                         <p style='margin: 5px 0;'><strong>Doctor:</strong> Dr. {doctorName}</p>
                         <p style='margin: 5px 0;'><strong>Specialty:</strong> {specialty}</p>
                     </div>
                 
                     <div style='background-color: #f9f9f9; padding: 15px; border-radius: 8px;'>
                         <table style='width: 100%; border-collapse: collapse;'>
                             <tr>
                                 <td style='width: 45%; vertical-align: top; padding: 10px; background-color: #fff5f5; border-left: 4px solid #e74c3c;'>
                                     <p style='margin: 0; color: #e74c3c; font-size: 0.8em; font-weight: bold;'>PREVIOUS SCHEDULE</p>
                                     <p style='margin: 5px 0; color: #777; text-decoration: line-through;'>{oldDate:dddd, dd MMM}</p>
                                     <p style='margin: 0; color: #777; text-decoration: line-through;'>{oldDate:hh:mm tt}</p>
                                 </td>
                                 
                                 <td style='width: 10%; text-align: center; font-size: 20px; color: #999;'> ➔ </td>
                 
                                 <td style='width: 45%; vertical-align: top; padding: 10px; background-color: #f0fff4; border-left: 4px solid #27ae60;'>
                                     <p style='margin: 0; color: #27ae60; font-size: 0.8em; font-weight: bold;'>NEW SCHEDULE</p>
                                     <p style='margin: 5px 0; font-weight: bold;'>{newDate:dddd, dd MMM yyyy}</p>
                                     <p style='margin: 0; font-weight: bold;'>{newDate:hh:mm tt}</p>
                                 </td>
                             </tr>
                         </table>
                     </div>
                 
                     <p style='margin-top: 20px; font-size: 0.9em; color: #666;'>
                         <i>Note: If this new timing doesn't suit you, please contact the clinic or manage your booking through the app.</i>
                     </p>
                 
                     <p>Best Regards,<br><strong>Elite Clinic Team</strong></p>
                 </div>";
        }

        public static string GetPaymentAndBookingConfirmation(string patientName,string doctorName, string specialty,DateTime date,
        decimal amount,string paymentMethod,int transactionId) 
        {
            return $@"
            <div style='font-family: Arial, sans-serif; direction: ltr; color: #333; max-width: 600px; border: 1px solid #eee; margin: 0 auto; padding: 20px; border-radius: 10px;'>
                <div style='text-align: center; border-bottom: 2px solid #f8f9fa; padding-bottom: 20px; margin-bottom: 20px;'>
                    <div style='background-color: #27ae60; color: white; width: 60px; height: 60px; line-height: 60px; border-radius: 50%; font-size: 30px; margin: 0 auto 10px;'>✓</div>
                    <h2 style='color: #27ae60; margin: 0;'>Payment Confirmed</h2>
                    <p style='color: #7f8c8d; margin: 5px 0;'>Transaction ID: #{transactionId}</p>
                </div>
            
                <p>Dear <strong>{patientName}</strong>,</p>
                <p>Your appointment has been officially confirmed and the payment was processed successfully.</p>
                
                <h3 style='font-size: 16px; color: #2c3e50; background-color: #f8f9fa; padding: 10px;'>Appointment Details</h3>
                <div style='padding: 0 10px; margin-bottom: 20px;'>
                    <p style='margin: 5px 0;'><strong>Doctor:</strong> Dr. {doctorName}</p>
                    <p style='margin: 5px 0;'><strong>Specialty:</strong> {specialty}</p>
                    <p style='margin: 5px 0;'><strong>Date:</strong> {date:dddd, dd MMMM yyyy}</p>
                    <p style='margin: 5px 0;'><strong>Time:</strong> {date:hh:mm tt}</p>
                </div>
            
                <h3 style='font-size: 16px; color: #2c3e50; background-color: #f8f9fa; padding: 10px;'>Billing Information</h3>
                <div style='padding: 0 10px;'>
                    <table style='width: 100%; border-collapse: collapse;'>
                        <tr>
                            <td style='padding: 8px 0; color: #7f8c8d;'>Payment Method</td>
                            <td style='padding: 8px 0; text-align: right; font-weight: bold;'>{paymentMethod}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px 0; color: #7f8c8d;'>Consultation Fees</td>
                            <td style='padding: 8px 0; text-align: right; font-weight: bold;'>{amount:N2} EGP</td>
                        </tr>
                        <tr style='border-top: 2px solid #eee; font-size: 18px;'>
                            <td style='padding: 15px 0; font-weight: bold; color: #2c3e50;'>Amount Paid</td>
                            <td style='padding: 15px 0; text-align: right; font-weight: bold; color: #27ae60;'>{amount:N2} EGP</td>
                        </tr>
                    </table>
                </div>
            
                <div style='margin-top: 30px; padding: 15px; background-color: #fff3cd; border: 1px solid #ffeeba; border-radius: 5px; color: #856404; font-size: 13px;'>
                    <strong>Important:</strong> Please present this digital receipt at the clinic reception upon arrival.
                </div>
            
                <p style='margin-top: 25px; font-size: 14px; color: #95a5a6; text-align: center;'>
                    Thank you for choosing Elite Clinic.<br>
                    If you have any questions, contact us at support@eliteclinic.com
                </p>
            </div>";
        }
        public static string GetPatientCancellationEmail(string patientName, string doctorName, string specialty, DateTime date)
        {
            return $@"
            <div style='font-family: Arial, sans-serif; direction: ltr; color: #333; max-width: 600px; border: 1px solid #eee; padding: 20px; border-radius: 10px;'>
                <h2 style='color: #e74c3c;'>Appointment Cancelled</h2>
                <p>Dear <strong>{patientName}</strong>,</p>
                <p>As per your request, your appointment has been cancelled successfully.</p>
                
                <div style='background-color: #f8f9fa; padding: 15px; border-left: 5px solid #e74c3c; margin: 20px 0;'>
                    <p style='margin: 5px 0;'><strong>Doctor:</strong> Dr. {doctorName}</p>
                    <p style='margin: 5px 0;'><strong>Specialty:</strong> {specialty}</p>
                    <p style='margin: 5px 0;'><strong>Date:</strong> {date:dddd, dd MMMM yyyy}</p>
                    <p style='margin: 5px 0;'><strong>Time:</strong> {date:hh:mm tt}</p>
                </div>
            
                <p>We understand that plans change. If you would like to book a new appointment, you can do so at any time through our website or app.</p>
                
                <div style='text-align: center; margin-top: 30px;'>
                    <a href='https://elite-clinic.com/book' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Book New Appointment</a>
                </div>
            
                <p style='margin-top: 25px; font-size: 0.9em; color: #777;'>Best Regards,<br><strong>Elite Clinic Team</strong></p>
            </div>";
        }

        public static string GetAutoCancellationEmail(string patientName, string doctorName, string specialty, DateTime date)
        {
            return $@"
            <div style='font-family: Arial, sans-serif; direction: ltr; color: #333; max-width: 600px; border: 1px solid #eee; margin: 0 auto; padding: 20px; border-radius: 10px;'>
                <div style='text-align: center; margin-bottom: 20px;'>
                    <h2 style='color: #d35400; margin: 0;'>Appointment Reservation Expired</h2>
                    <p style='color: #7f8c8d; font-size: 0.9em;'>Payment Timeout Reached</p>
                </div>
            
                <p>Dear <strong>{patientName}</strong>,</p>
                <p>We're writing to inform you that your recent reservation with <strong>Dr. {doctorName}</strong> has expired because the payment was not completed within the required 60-minute window.</p>
                
                <div style='background-color: #f8f9fa; padding: 15px; border-left: 5px solid #d35400; margin: 20px 0;'>
                    <p style='margin: 5px 0;'><strong>Doctor:</strong> Dr. {doctorName}</p>
                    <p style='margin: 5px 0;'><strong>Specialty:</strong> {specialty}</p>
                    <p style='margin: 5px 0;'><strong>Was Scheduled for:</strong> {date:dddd, dd MMMM} at {date:hh:mm tt}</p>
                </div>
            
                <div style='background-color: #fff3cd; border: 1px solid #ffeeba; padding: 15px; color: #856404; border-radius: 5px; margin-bottom: 20px;'>
                    <strong style='display: block; margin-bottom: 5px;'>Why was it cancelled?</strong>
                    To ensure all our patients have a fair chance to book available slots, unpaid reservations are automatically released after 1 hour.
                </div>
            
                <p><strong>Don't worry!</strong> Your preferred slot might still be available. You can re-book it now and complete the payment to secure your visit.</p>
                
                <div style='text-align: center; margin-top: 30px; margin-bottom: 30px;'>
                    <a href='https://elite-clinic.com/book' 
                       style='background-color: #28a745; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
                       Re-book This Appointment
                    </a>
                </div>
            
                <hr style='border: 0; border-top: 1px solid #eee;'>
                <p style='font-size: 0.8em; color: #999; text-align: center;'>
                    If you encountered any technical difficulties during the payment process, please reply to this email or contact our support team.
                </p>
                
                <p style='font-size: 0.9em; margin-top: 20px;'>Best Regards,<br><strong>Elite Clinic Team</strong></p>
            </div>";
        }

        public static string GetNoShowNotice(string patientName, string doctorName, string specialty, DateTime date)
        {
            return $@"
            <div style='font-family: Arial, sans-serif; direction: ltr; color: #333; max-width: 600px; border: 1px solid #eee; margin: 0 auto; padding: 20px; border-radius: 10px;'>
                
                <div style='text-align: center; margin-bottom: 20px;'>
                    <div style='background-color: #f39c12; color: white; width: 60px; height: 60px; line-height: 60px; border-radius: 50%; font-size: 30px; margin: 0 auto 10px;'>!</div>
                    <h2 style='color: #2c3e50; margin: 0;'>We Missed You!</h2>
                    <p style='color: #7f8c8d;'>Appointment No-Show Notification</p>
                </div>
            
                <p>Dear <strong>{patientName}</strong>,</p>
                <p>We missed you today at <strong>Elite Clinic</strong>. We're writing to follow up because you were unable to attend your scheduled appointment with <strong>Dr. {doctorName}</strong>.</p>
                
                <div style='background-color: #fff9f0; padding: 15px; border-left: 5px solid #f39c12; margin: 20px 0;'>
                    <p style='margin: 5px 0;'><strong>Doctor:</strong> Dr. {doctorName}</p>
                    <p style='margin: 5px 0;'><strong>Specialty:</strong> {specialty}</p>
                    <p style='margin: 5px 0;'><strong>Missed Date:</strong> {date:dddd, dd MMMM yyyy}</p>
                    <p style='margin: 5px 0;'><strong>Missed Time:</strong> {date:hh:mm tt}</p>
                </div>
            
                <p>We understand that unexpected things can happen. However, to ensure our doctors can help as many patients as possible, we kindly ask you to let us know in advance if you cannot make it next time.</p>
            
                <div style='background-color: #e8f4fd; padding: 15px; border-radius: 5px; color: #31708f; margin-bottom: 20px;'>
                    <strong>Would you like to reschedule?</strong><br>
                    Health is a priority. You can easily book a new slot that fits your schedule.
                </div>
            
                <div style='text-align: center; margin-top: 30px; margin-bottom: 30px;'>
                    <a href='https://elite-clinic.com/book' 
                       style='background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
                       Schedule a New Appointment
                    </a>
                </div>
            
                <hr style='border: 0; border-top: 1px solid #eee;'>
                <p style='font-size: 0.8em; color: #999; text-align: center;'>
                    If you actually attended or believe this is a mistake, please contact our reception immediately.
                </p>
                
                <p style='font-size: 0.9em; margin-top: 20px;'>Best Regards,<br><strong>Elite Clinic Team</strong></p>
            </div>";
        }

        public static string GetMedicalReportEmail(string patientName, string doctorName, string specialty, string diagnosis, string description, List<Prescription> medicines, string? additionalNotes)
        {
            // 1. بناء جدول الأدوية (الروشتة) مع المواعيد والمدة
            var medicinesRows = new StringBuilder();
            if (medicines != null && medicines.Any())
            {
                foreach (var med in medicines)
                {
                    // حساب مدة العلاج بالأيام (اختياري للعرض)
                    var durationDays = (med.EndDate - med.StartDate).Days;

                    medicinesRows.Append($@"
                <tr style='border-bottom: 1px solid #eee; font-size: 0.9em;'>
                    <td style='padding: 12px; color: #2c3e50;'>
                        <strong>{med.MedicationName}</strong><br>
                        <span style='font-size: 0.8em; color: #27ae60;'>Period: {med.StartDate:dd/MM} to {med.EndDate:dd/MM}</span>
                    </td>
                    <td style='padding: 12px;'>{med.Dosage}</td>
                    <td style='padding: 12px;'>
                        {med.Frequency}<br>
                        <small style='color: #7f8c8d;'>({durationDays} days)</small>
                    </td>
                    <td style='padding: 12px; font-size: 0.85em; color: #666;'>
                        {(string.IsNullOrEmpty(med.SpecialInstructions) ? "-" : med.SpecialInstructions)}
                    </td>
                </tr>");
                }
            }

            return $@"
            <div style='font-family: Arial, sans-serif; direction: ltr; color: #333; max-width: 800px; border: 1px solid #ddd; margin: 0 auto; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.1);'>
                
                <div style='background-color: #2c3e50; color: white; padding: 20px; text-align: center;'>
                    <h2 style='margin: 0;'>Elite Clinic - Medical Visit Report</h2>
                    <p style='margin: 5px 0; opacity: 0.8; font-size: 0.9em;'>Full Visit Summary & E-Prescription</p>
                </div>
            
                <div style='padding: 30px;'>
                    <table style='width: 100%; margin-bottom: 25px; font-size: 0.95em; border-bottom: 1px solid #eee; padding-bottom: 10px;'>
                        <tr>
                            <td><strong>Patient:</strong> {patientName}</td>
                            <td style='text-align: right;'><strong>Date:</strong> {DateTime.Now:dd MMM yyyy}</td>
                        </tr>
                        <tr>
                            <td><strong>Doctor:</strong> Dr. {doctorName}</td>
                            <td style='text-align: right;'><strong>Specialty:</strong> {specialty}</td>
                        </tr>
                    </table>
            
                    <div style='background-color: #f8f9fa; border-left: 4px solid #3498db; padding: 15px; margin-bottom: 25px;'>
                        <h3 style='margin-top: 0; color: #3498db; font-size: 1.1em;'>I. Clinical Assessment</h3>
                        <p style='margin-bottom: 5px;'><strong>Diagnosis:</strong> {diagnosis}</p>
                        <p style='margin-bottom: 5px;'><strong>Clinical Description:</strong></p>
                        <div style='background-color: #fff; padding: 10px; border-radius: 4px; border: 1px solid #eee;'>{description}</div>
                    </div>
            
                    <div style='margin-bottom: 25px;'>
                        <h3 style='color: #27ae60; font-size: 1.1em; border-bottom: 2px solid #27ae60; padding-bottom: 5px;'>II. Prescription & Medication Schedule</h3>
                        <table style='width: 100%; border-collapse: collapse; margin-top: 10px;'>
                            <thead>
                                <tr style='background-color: #f0fff4; border-bottom: 2px solid #eee; font-size: 0.9em;'>
                                    <th style='padding: 12px; text-align: left;'>Medication & Period</th>
                                    <th style='padding: 12px; text-align: left;'>Dosage</th>
                                    <th style='padding: 12px; text-align: left;'>Frequency</th>
                                    <th style='padding: 12px; text-align: left;'>Instructions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {medicinesRows}
                            </tbody>
                        </table>
                    </div>
            
                    {(!string.IsNullOrWhiteSpace(additionalNotes) ? $@"
                    <div style='background-color: #fff9e6; border-left: 4px solid #f1c40f; padding: 15px; margin-bottom: 25px;'>
                        <h3 style='margin-top: 0; color: #856404; font-size: 1.1em;'>III. Doctor's Instructions</h3>
                        <p style='margin: 0;'>{additionalNotes}</p>
                    </div>" : "")}
            
                    <div style='text-align: center; margin-top: 40px; border-top: 1px solid #eee; padding-top: 20px;'>
                        <p style='font-size: 0.85em; color: #7f8c8d;'>
                            This is an automated medical record. Please follow the medication schedule strictly.
                        </p>
                        <p style='color: #2c3e50; font-weight: bold;'>Wishing you a speedy recovery!</p>
                    </div>
                </div>
            </div>";
        }

        public static string GetEmailConfirmationTemplate(string name, string userName, string email, string confirmationLink, string? specialty = null)
        {
            // 1. تحديد نوع الحساب ورسالة الترحيب بناءً على وجود التخصص
            bool isDoctor = !string.IsNullOrEmpty(specialty);
            string accountType = isDoctor ? "Doctor" : "Patient";
            string welcomeSubtitle = isDoctor ? "Welcome to our Medical Team 👨‍⚕️" : "Welcome to our family 🏡";

            // 2. تجهيز سطر التخصص (يظهر فقط للدكتور)
            string specialtyRow = isDoctor
                ? $@"<div class='details-item'><strong>Specialty:</strong> {specialty}</div>"
                : "";

            // 3. تعديل الاسم (لو دكتور نضيف Dr. قبل الاسم)
            string displayName = isDoctor && !name.StartsWith("Dr.", StringComparison.OrdinalIgnoreCase)
                ? $"Dr. {name}"
                : name;

            return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f6f8; margin: 0; padding: 0; }}
                .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.05); overflow: hidden; border: 1px solid #e1e4e8; }}
                .header {{ background-color: #0d6efd; color: #ffffff; padding: 30px; text-align: center; }}
                .header h1 {{ margin: 0; font-size: 24px; font-weight: 600; }}
                .content {{ padding: 30px; color: #333333; line-height: 1.6; }}
                .details-box {{ background-color: #f8f9fa; border-left: 5px solid #0d6efd; padding: 15px; margin: 20px 0; border-radius: 4px; }}
                .details-item {{ margin-bottom: 10px; font-size: 14px; }}
                .btn-container {{ text-align: center; margin-top: 30px; margin-bottom: 20px; }}
                .btn {{ background-color: #0d6efd; color: #ffffff !important; text-decoration: none; padding: 12px 30px; border-radius: 5px; font-weight: bold; font-size: 16px; display: inline-block; transition: background-color 0.3s; }}
                .btn:hover {{ background-color: #0b5ed7; }}
                .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #6c757d; border-top: 1px solid #e1e4e8; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Elite Clinic System</h1>
                    <p style='margin: 5px 0 0; opacity: 0.9;'>{welcomeSubtitle}</p>
                </div>

                <div class='content'>
                    <h2 style='color: #2c3e50; margin-top: 0;'>Hi, {displayName}! 👋</h2>
                    <p>Thanks for joining Elite Clinic. We are excited to have you on board.</p>
                    
                    <p>Here is a summary of your registration details:</p>
                    
                    <div class='details-box'>
                        <div class='details-item'><strong>User Name:</strong> {userName}</div>
                        <div class='details-item'><strong>Email Address:</strong> {email}</div>
                        <div class='details-item'><strong>Account Type:</strong> {accountType}</div>
                        {specialtyRow}
                    </div>

                    <p>Please confirm your email address to activate your account.</p>

                    <div class='btn-container'>
                        <a href='{confirmationLink}' class='btn'>Confirm My Account</a>
                    </div>
                    
                    <p style='font-size: 13px; color: #999;'>If the button doesn't work, copy and paste this link into your browser:<br>
                    <a href='{confirmationLink}' style='color: #0d6efd; word-break: break-all;'>{confirmationLink}</a></p>
                </div>

                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} Elite Clinic. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";
        }
       
        public static string GetResetPasswordTemplate(string email, string resetLink)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f6f8; margin: 0; padding: 0; }}
            .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.05); overflow: hidden; border: 1px solid #e1e4e8; }}
            .header {{ background-color: #dc3545; color: #ffffff; padding: 30px; text-align: center; }} /* خليت اللون أحمر عشان ده Reset Password */
            .header h1 {{ margin: 0; font-size: 24px; font-weight: 600; }}
            .content {{ padding: 30px; color: #333333; line-height: 1.6; }}
            .alert-box {{ background-color: #fff3cd; border-left: 5px solid #ffc107; padding: 15px; margin: 20px 0; border-radius: 4px; color: #856404; }}
            .btn-container {{ text-align: center; margin-top: 30px; margin-bottom: 20px; }}
            .btn {{ background-color: #dc3545; color: #ffffff !important; text-decoration: none; padding: 12px 30px; border-radius: 5px; font-weight: bold; font-size: 16px; display: inline-block; transition: background-color 0.3s; }}
            .btn:hover {{ background-color: #bb2d3b; }}
            .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #6c757d; border-top: 1px solid #e1e4e8; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Elite Clinic System</h1>
                <p style='margin: 5px 0 0; opacity: 0.9;'>Password Reset Request</p>
            </div>

            <div class='content'>
                <h2 style='color: #2c3e50; margin-top: 0;'>Hello,</h2>
                <p>We received a request to reset the password for the account associated with <strong>{email}</strong>.</p>
                
                <div class='alert-box'>
                    <strong>Security Notice:</strong> If you did not request a password reset, please ignore this email. Your password will remain unchanged.
                </div>

                <p>You can reset your password by clicking the button below:</p>

                <div class='btn-container'>
                    <a href='{resetLink}' class='btn'>Reset My Password</a>
                </div>
                
                <p style='font-size: 13px; color: #999;'>If the button doesn't work, copy and paste this link into your browser:<br>
                <a href='{resetLink}' style='color: #dc3545; word-break: break-all;'>{resetLink}</a></p>
            </div>

            <div class='footer'>
                <p>&copy; {DateTime.Now.Year} Elite Clinic. All rights reserved.</p>
                <p>This link is valid for a limited time only.</p>
            </div>
        </div>
    </body>
    </html>";
        }
    }
}
