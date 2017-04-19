using System.Collections;
using System.Collections.Generic;
using Eto.Forms;
using NTouchTypeTrainer.ViewModels;
using NTouchTypeTrainer.Views.Controls;

namespace NTouchTypeTrainer.Common.DataBinding
{
    public static class BindingExtensions
    {
        public static IList BindToKeyViewModelDataContext(
            this HardwareKeyControl keyControl,
            DualBindingMode mode = DualBindingMode.TwoWay,
            KeyViewModel defaultControlValue = default(KeyViewModel),
            KeyViewModel defaultContextValue = default(KeyViewModel))
        {
            var bindings = new List<object>();

            if (keyControl != null)
            {
                bindings.Add(
                    keyControl.BindDataContext(
                        ctrl => ctrl.Text,
                        (KeyViewModel vm) => vm.Name,
                        mode,
                        defaultControlValue?.Name,
                        defaultContextValue?.Name));

                bindings.Add(
                    keyControl.BindDataContext(
                        ctrl => ctrl.BackgroundColor,
                        (KeyViewModel vm) => vm.Color,
                        mode,
                        defaultControlValue?.Color,
                        defaultContextValue?.Color));
            }

            return bindings;
        }
    }
}