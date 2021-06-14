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

		public static void CreateNotifications(Course course, List<Reception> receptions)
		{
			foreach (var reception in receptions) {
				NotificationCenter.Current.Show(
					GetNotificationRequest(reception, course.StartDate + reception.Time)
				);
			}
		}

		public static void UpdateNotifications(Course course, List<Reception> receptions)
		{
			DeleteNotifications(receptions);
			foreach (var reception in receptions) {
				var time = GetNextReceptionTime(course, reception);
				if (time != null) {
					NotificationCenter.Current.Show(
						GetNotificationRequest(reception, time)
					);
				}
			}
		}

		public static void DeleteNotifications(List<Reception> receptions)
		{
			foreach (var reception in receptions) {
				NotificationCenter.Current.Cancel(reception.Id);
			}
		}

		private static NotificationRequest GetNotificationRequest(Reception reception, DateTime? time)
		{
			var repeatInterval = RepeatInterval;
			var cancelTime = time + RepeatInterval * RepeatCount;
			return new NotificationRequest() {
				NotificationId = reception.Id,
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

		private static DateTime? GetNextReceptionTime(Course course, Reception reception)
		{
			return null;
		}
	}
}
