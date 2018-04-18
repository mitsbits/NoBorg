using System;
using Borg.Cms.Basic.Backoffice.BackgroundJobs;
using Borg.MVC.Services.Editors;
using Borg.Platform.EF.CMS;
using Hangfire.Storage;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.ViewModels
{

        public class ComponentSheduleViewModel
        {
            public int ComponentId { get; set; }
            public (ComponentJobScheduleState row, JobData job, StateData state)[] Records { get; set; }

            public ComponentPublishOperationScheduleCommand AddPublishOperationScheduleCommand ()=> new ComponentPublishOperationScheduleCommand()
            {
                ComponentId = ComponentId,
                Direction = new EnumDropDown(typeof(ComponentPublishOperation.OperationDirection), ComponentPublishOperation.OperationDirection.Up),
                TriggerDate = DateTimeOffset.Now.AddHours(1)
            };
        }

}