using System.Collections.Generic;

using MedicineSchedule.Models;

using Plugin.LocalNotification;

namespace MedicineSchedule.Services
{
	public static class NotificationManager
	{
		public static void CreateNotifications(Course course, List<Reception> receptions)
		{
			foreach (var reception in receptions) {
				var notification = new NotificationRequest() {
					NotificationId = reception.Id,
					Title = "Эй, дед!",
					Description = "Прими таблетки!",
					Schedule = new NotificationRequestSchedule() {
						Repeats = NotificationRepeat.No,
						NotifyTime = course.StartDate + reception.Time,
					},
					Android = new AndroidOptions() {
						Priority = NotificationPriority.High,
					}
				};
				NotificationCenter.Current.Show(notification);
			}
		}

		public static void UpdateNotifications(Course course, List<Reception> receptions)
		{

		}

		public static void DeleteNotifications(Course course, List<Reception> receptions)
		{

		}
	}
}
