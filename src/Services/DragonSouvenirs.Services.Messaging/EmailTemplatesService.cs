﻿namespace DragonSouvenirs.Services.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Models;

    public class EmailTemplatesService : IEmailTemplatesSender
    {
        public string Order(string orderTitle, string shippingAddress, string clientFullName, string invoiceNumber, decimal totalPrice, ICollection<OrderProduct> orderProducts, decimal deliveryPrice)
        {
            var sb = new StringBuilder();
            sb.Append($"<!DOCTYPE html>\r\n<html lang=\"en\">    \r\n    <head>\r\n        <meta charset=\"UTF-8\">\r\n        <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n        <title>Document</title>\r\n        <style>\r\n            .footer {{\r\n              max-width: 800px;\r\n              background-color: rgb(207, 206, 206);\r\n              margin: auto;\r\n              padding: 30px;\r\n              border: 1px solid #eee;\r\n              box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);\r\n              font-size: 16px;\r\n              line-height: 24px;\r\n              font-family: \"Helvetica Neue\", \"Helvetica\", Helvetica, Arial, sans-serif;\r\n              color: #555;\r\n            }}\r\n            \r\n            .footer table {{\r\n              width: 100%;\r\n              line-height: 30px;\r\n              text-align: center;\r\n            }}\r\n            \r\n            .footer table td {{\r\n              padding: 5px;\r\n              vertical-align: top;\r\n            }}\r\n            \r\n            .footer table tr td:nth-child(2) {{\r\n              text-align: center;\r\n            }}\r\n            \r\n            .footer table tr.top table td {{\r\n              padding-bottom: 20px;\r\n            }}\r\n            \r\n            .footer table tr.top table td.title {{\r\n              font-size: 45px;\r\n              line-height: 45px;\r\n              color: #333;\r\n              text-align: center;\r\n            }}\r\n            \r\n            .invoice-box {{\r\n              max-width: 800px;\r\n              background-color: rgb(255, 255, 255);\r\n              margin: auto;\r\n              padding: 30px;\r\n              border: 1px solid #eee;\r\n              box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);\r\n              font-size: 16px;\r\n              line-height: 24px;\r\n              font-family: \"Helvetica Neue\", \"Helvetica\", Helvetica, Arial, sans-serif;\r\n              color: #555;\r\n            }}\r\n            .invoice-box2 {{\r\n              max-width: 800px;\r\n              background-color: rgb(255, 255, 255);\r\n              margin: auto;\r\n              padding: 30px;\r\n              border: 1px solid #eee;\r\n              box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);\r\n              font-size: 16px;\r\n              line-height: 24px;\r\n              font-family: \"Helvetica Neue\", \"Helvetica\", Helvetica, Arial, sans-serif;\r\n              color: #555;\r\n            }}\r\n            \r\n            .invoice-box table {{\r\n              width: 100%;\r\n              line-height: inherit;\r\n              text-align: left;\r\n            }}\r\n            \r\n            .invoice-box table td {{\r\n              padding: 5px;\r\n              vertical-align: top;\r\n            }}\r\n            \r\n            .invoice-box table tr td:nth-child(2) {{\r\n              text-align: right;\r\n            }}\r\n            \r\n            .invoice-box table tr.top table td {{\r\n              padding-bottom: 20px;\r\n            }}\r\n            \r\n            .invoice-box table tr.top table td.title {{\r\n              font-size: 45px;\r\n              line-height: 45px;\r\n              color: #333;\r\n              text-align: left;\r\n            }}\r\n            \r\n            .invoice-box2 table {{\r\n              width: 100%;\r\n              line-height: inherit;\r\n              text-align: left;\r\n            }}\r\n            \r\n            .invoice-box2 table tr.heading td {{\r\n              background: #eee;\r\n              border-bottom: 1px solid #ddd;\r\n              font-weight: bold;\r\n            }}\r\n            \r\n            .invoice-box2 table tr.details td {{\r\n              padding-bottom: 1px;\r\n            }}\r\n            .invoice-box2 table tr.details.last td {{\r\n              border-bottom: 1px solid rgb(0, 0, 0);\r\n            }}\r\n            .invoice-box2 table tr.item td {{\r\n              border-bottom: 1px solid rgb(0, 0, 0);\r\n            }}\r\n        </style>\r\n    </head>\r\n    <body>\r\n        <div class=\"footer\">\r\n            <div class=\"invoice-box\">\r\n                <table>\r\n                    <tr>\r\n                        <td><img src=\"http://cdn.mcauto-images-production.sendgrid.net/54fdd2657f30071c/48472401-ea1d-4369-b551-747442afb243/159x51.png\"></td>\r\n                        <td style=\"text-align:right;\">{DateTime.Now:yyyy MMMM dd}</td>\r\n                    </tr>\r\n                </table>\r\n                <div align=\"center\">\r\n                    <img src=\"http://cdn.mcauto-images-production.sendgrid.net/54fdd2657f30071c/c6ccc20f-40c1-4849-96e4-17368660f008/250x250.gif\" alt=\"\">\r\n                </div>\r\n                <p style=\"text-align: left;\">{orderTitle}</p>\r\n            </div>\r\n            <div class=\"invoice-box2\">\r\n                <table>\r\n                    <tr class=\"heading\">\r\n                        <td colspan=1>Продукт</td>\r\n                        <td colspan=2>Цена</td>\r\n                    </tr>");
            foreach (var orderProduct in orderProducts)
            {
                sb.Append($"<tr class=\"details\">\r\n                        <td colspan=1>{orderProduct.Quantity} х {orderProduct.Product.Name}</td>\r\n                        <td colspan=2>{orderProduct.Price * orderProduct.Quantity} лв.</td>\r\n                    </tr>");
            }

            sb.Append($"<tr class=\"details\">\r\n                        <td colspan=1>Доставка</td>\r\n                        <td colspan=2>{deliveryPrice} лв.</td>\r\n                    </tr>\r\n                    <tr class=\"item\">\r\n                        <td colspan=1><strong>Total</strong></td>\r\n                        <td colspan=2><strong>{totalPrice + deliveryPrice} лв.</strong></td>\r\n                    </tr>\r\n                </table>\r\n            </div>\r\n            <div class=\"invoice-box\">\r\n                <table cellpadding=\"0\" cellspacing=\"0\">\r\n                    <tr class=\"top\">\r\n                        <table>\r\n                            <tr class=\"title\">\r\n                                <td colspan=\"1\"> \r\n                                    <b>Адрес на фактуриране</b>\r\n                                    <br>{clientFullName}\r\n                                    <br>{shippingAddress}<br>{invoiceNumber}\r\n                                </td><td colspan=\\\"2\\\"> \r\n                                    <b>Адрес на доставака</b>\r\n                                    <br>{clientFullName}\r\n                                    <br>{shippingAddress}\r\n{invoiceNumber}                                </td>\r\n                            </tr>\r\n                        </table>\r\n                    </tr>\r\n                </table>\r\n            </div>\r\n            <table cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\r\n                <tr class=\\\"top\\\">\r\n                    <table>\r\n                        <tr class=\\\"title\\\">\r\n                            <td>\r\n                                DragonSouvenirs<br> Varna, Bulgaria, 9003 +359 895 53 34 22\r\n                            </td>\r\n                        </tr>\r\n                    </table>\r\n                </tr>\r\n            </table>\r\n        </div>\r\n    </body>\r\n</html>\"");

            return sb.ToString().Trim();
        }
    }
}