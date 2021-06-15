using System;
using System.Collections.Generic;

using MedicineSchedule.Models;

using Plugin.LocalNotification;

namespace MedicineSchedule.Services
{
	public static class NotificationManager
	{
		private static readonly TimeSpan repeatInterval = new TimeSpan(0, 10, 0);
		private static readonly int repeatCount = 3;

		public static void CreateNotification(NextReceptionInfo info)
		{
			NotificationCenter.Current.Show(new NotificationRequest() {
				NotificationId = info.Id,
				Title = "Эй, дед!",
				Description = "Прими таблетки, а то получишь по жопе!",
				Schedule = new NotificationRequestSchedule() {
					Repeats = NotificationRepeat.TimeInterval,
					NotifyTime = info.NextDateTime,
					NotifyRepeatInterval = repeatInterval,
					NotifyAutoCancelTime = info.NextDateTime + repeatInterval * repeatCount,
				},
				Android = new AndroidOptions() {
					Priority = NotificationPriority.High,
				}
			});
		}

		public static void DeleteNotification(NextReceptionInfo info)
		{
			NotificationCenter.Current.Cancel(info.Id);
		}
	}
}
