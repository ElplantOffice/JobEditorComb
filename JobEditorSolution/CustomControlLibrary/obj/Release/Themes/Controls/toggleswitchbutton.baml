   M S B A M L   `   `   `  ???? O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  O KCustomControlLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:CustomControlLibraryCustomControlLibrary R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  ?? MergedDictionaries ?? Source.   (CustomControlLibrary.PercentageConverter   PercentageConverter-  'CustomControlLibrary.ToggleSwitchButton ?? 
TargetType ?? Property ?? Value ?? Path ?? RelativeSource ?? AncestorType * &ToggleSwitchButtonBackgroundThemeBrush & "ToggleSwitchButtonBorderThemeBrush $  ToggleSwitchButtonHeadThemeBrush ?? 
TargetType	 ??Name
 J? Opacity ?? Padding'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard ?? TargetProperty ?? 
TargetName O? KeyTime O? Value R? KeyTime  System.Windows.Visibility   Visible R? Value   Transitions%  System.Windows.VisualTransition   GeneratedDuration	   To   
Storyboard g? Duration	 W? To W? EasingFunction/  )System.Windows.Media.Animation.CircleEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode W?Name4	  .System.Windows.Media.Animation.ExponentialEase 	  Exponent  U?Name9
  3System.Windows.Media.Animation.EasingDoubleKeyFrame!   	Collapsed" ?Name# ? UseLayoutRounding$ ??Name ' #ToggleSwitchButtonCheckedThemeBrush ' #ToggleSwitchButtonPressedThemeBrush + 'ToggleSwitchButtonPointerOverThemeBrush% ?? 	Converter& ?? ElementName' ?? ConverterParameter(   SwitchForeground?? D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     1System&clr-namespace:System;assembly=mscorlib  ,local"clr-namespace:CustomControlLibrary    ?? $@ :/CustomControlLibrary;component/Themes/General/Colors.xaml???? $? 9/CustomControlLibrary;component/Themes/General/Fonts.xaml??%_  &	        '       7  7  7  7  7  7  7      ?? "  .???  ? ?? ?? $ 
Background? ?? *FindAncestor+" ???  ? ??8   ??  ? ??8  ??  ?@ SwitchForeground8  ??  ? ??$ 85????  ? ??$ 36????  ? ?? ?? "  .R??? $
	 Root??#??z??#??z??#
 z
 # z??   CommonStates??.   
Normal??  Disabled??. ?? .??U? $ Opacity?$ 
SwitchRoot??.\??? $ 0??$	 0.5Q?  	MouseOver??. ?? .??T? $ (UIElement.Visibility)?$ hover??..?i? $ 0??# Z   Pressed??. ?? .??T? $ (UIElement.Visibility)?$ pressed??..?i? $ 0??# Z   CheckStates??   $ 0:0:0.05D?$ 	Unchecked??  $ 0:0:0.05D?$ Checked??. ?? .??W? $ 0:0:0.05D?# z??$ (TranslateTransform.X)?$ SwitchThumbTranslate??   $ EaseIn=?.   Checked??. ?? .??W? $ CheckedDoubleAnimation??$ BackgroundTranslation??$ (TranslateTransform.X)?$ 0D?$ 47X? 	  $ 15Q?$ EaseOut=?U? $  CheckedEasingKeyFrame??$U O(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)?$ SwitchThumb??.\?
  $ 0??$ 47Q?  Dragging??  	Unchecked??. ?? .??W? $ BackgroundTranslation??$ (TranslateTransform.X)?$ 0D?$ 0X?T? $ (UIElement.Visibility)?$ checked??..?i? $ 0??# Z! ._?? $" 
SwitchRoot??$
??Auto??$
??Auto??# . ??Z? .+?? $" SwitchTrack??$
??Auto??.+?? $" SwitchBottom??#??z??#??z??.+??? $$ SwitchBackground??$
??Auto??$
??Auto??$??Stretch=?$??Stretch=?z?T? BackgroundTranslation???? $??2q?#??z??$??2q?._?? $??2q?.+??? $??0q?#??z??$??0q??? $	 checked??$??0q?8?? $y?Visible=?$??0q??? $	 pressed??$??0q?8?? $
 1Q?$y?	Collapsed=?$??0q??? $	 hover??$??0q?8?? $
 1Q?$y?	Collapsed=?$??0q??? $	 SwitchThumb??#??z??$??0q?$??0q?$
??Auto??$
??Left=????? 8%  $& 
SwitchRoot??$ ActualWidth?$' 30??z?V? .~??? ?? ?? T? SwitchThumbTranslate??._??? $	 ThumbCenter??#??z??$??2q?#??z( 