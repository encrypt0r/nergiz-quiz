using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
namespace NergizQuiz.UI
{
    public interface IAnimatedUserControl
    {
       Storyboard StartAnimation();
    }
}
