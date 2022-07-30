using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace C868
{
    class FixedPicker : Picker
    {
        public FixedPicker()
        {
            SelectedIndexChanged += FixedPicker_SelectedIndexChanged;
        }

        private void FixedPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvalidateMeasure();
        }
    }
}
