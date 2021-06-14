using System;
using System.Collections.Generic;

using MedicineSchedule.Models;

using Plugin.LocalNotification;

namespace MedicineSchedule.Services
{
	public static class NotificationManager
	{
		private static readonly TimeSpan RepeatInterval = new TimeSpan(0, 10, 0);
		private static readonly int RepeatCount = 3;

		public static void CreateNotification(int id, DateTime? date)
		{
			NotificationCenter.Current.Show(GetNotificationRequest(id, date));
		}

		public static void DeleteNotification(int id)
		{
			NotificationCenter.Current.Cancel(id);
		}

		public static DateTime? GetNextTime(Reception reception, Course course)
		{
			return null;
		}

		private static NotificationRequest GetNotificationRequest(int id, DateTime? time)
		{
			var repeatInterval = RepeatInterval;
			var cancelTime = time + RepeatInterval * RepeatCount;
			return new NotificationRequest() {
				NotificationId = id,
				Title = "Эй, дед!",
				Description = "Прими таблетки!",
				Schedule = new NotificationRequestSchedule() {
					Repeats = NotificationRepeat.TimeInterval,
					NotifyTime = time,
					NotifyRepeatInterval = repeatInterval,
					NotifyAutoCancelTime = cancelTime,
				},
				Android = new AndroidOptions() {
					Priority = NotificationPriority.High,
				}
			};
		}
	}
}
