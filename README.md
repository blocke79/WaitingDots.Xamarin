#Xamarin C# port of WaitingDots

![Loading animation](https://github.com/blocke79/WaitingDots.Xamarin/screenshot.gif)

Ported from https://github.com/tajchert/WaitingDots

Small library that provides... bouncing dots. This feature is used in number of messaging apps (such as Hangouts or Messenger), and lately in Android TV (for example when connecting to Wifi).

Add this to your parent layout

"xmlns:dots="http://schemas.android.com/apk/res-auto""


        <blocke.waitingdots.DotsTextView
            android:layout_width="wrap_content"
            android:layout_height="wrap_content"
            android:id="@+id/dots"
            android:textSize="45sp"
            android:textColor="@android:color/primary_text_light"
            dots:autoplay="false"
            dots:period="175" />
			
```
All additional parameters are optional.

List of useful methods:

dots.Stop();
dots.Start();

dots.Hide();
dots.Show();

dots.HideAndStop();
dots.ShowAndPlay();

dots.IsHide();
dots.IsPlaying();
```