   M S B A M L   `   `   `  ����E  AQlySanBong, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:QlySanBong.ResourceXAMLQlySanBong.ResourceXAML  5      *    $QlySanBong.ResourceXAML.FieldControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      1local%clr-namespace:QlySanBong.ResourceXAML  5      -   5          >�Name$  ucField��5      $	��247��5      $	��200��5         FieldVM � DataContext# [  5      
�������
���  ��$��	0,0,10,10q�5      .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ 	MouseMove��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *HoverCommand+   CommandParameter 5   $   ��  �� ElementName$ ucField��5        5   
   $ 
MouseLeave��6   . 5         6$   �� *LeaveCommand+ 5   $   �� $ ucField��5      .��5      � .+�5   
   � -   5         �Name$ grdMask��5      $y�Hidden=�5       	 K� ZIndex$	 1��5!      $	��162��5"      $��0q�5#      $	��270��5$      $	��Top=�5%      ��5&      �� 5'      
 �� Color$
 Black��5(       �� Opacity$	 0.6Q�5)      5+      � 5,   
   -   5        $ icnError��5.      $	 2��5-      $��50��5/      $��50��50      $
��Left=�51      $	��Top=�52      $��20,8,0,0q�53      $y�Visible=�54      ��55      Z� .+�56      �� $��50��57      $��50��58      
���  ����59      Z� ��5:      Z� .��5<      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Rhombus=�5=      $��Center=�5>      $��20��5?      $��20��5@      5A      �� 5B      $��50��5C      $��50��5D      $��1,0,0,0q�5E      
���������5F      Z� ��5G      Z� .��5I        $ Exclamation=�6*   5J      5K      � 5L   
   -   5        $ grdField��5M      $��0q�5N      $	��160��5O      $	��500��5P      $	��Top=�5Q      $��Center=�5R      .+�5S      �� $��/Image/41290.jpg��5T      $��	0,0,0.4,0q�6+   5V      5W      �� 5X   
   -   5         ��Name$ 
txbIdField��5Y      $
��Left=�5Z      $��Bottom=�5[      $y�Hidden=�5\      $��16(�5]      �� 5^   
   -   5        $ txbFieldName��5_      $
��Left=�5`      $��10,0,0,8q�5a      $
��Wrap=�5b      $��Sân 1��5c      $��Bottom=�5d      $��SemiBold"�5e      $��16(�5f      $��WordEllipsis=�5g      $	��125��5h      $
��21.6��5i      �� 5j   
   -   5        $ txbFieldType��5k      $��Right=�5l      $��0,0,10,8q�5m      $
��Wrap=�5n      $��Sân 5 người��5o      $��Bottom=�5p      $��SemiBold"�5q      $��16(�5r      �� 5s   
   -   5         ��Name$ btnEdit��5u      $	 2��5t      $y�Visible=�5v      
����  �  MaterialDesignRaisedButton#��[ 5z       � ToolTip$ Sửa thông tin��5~      $��50��5      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 6��5�      $��	0,58,50,0q�5�      $��16(�5�      $	��Top=�5�      $��50��5�      $��Center=�5�      $��0q�5�      $��SemiBold"�5�      ��5x      �� *EditCardFieldCommand+��5y      �� $ ucField����5{      Z� ��5|      Z�  � FocusVisualStyle 5}      Z� .��5�        $ SquareEditOutline=�5�      $��34��5�      $��34��5�      5�      �� 5�   
   -	   5        $ 	btnDelete��5�      $	 2��5�      $y�Visible=�5�      
���  ��#��[ 5�      $
 Xoá��5�      $��50��5�      $ 6��5�      $��	50,58,0,0q�5�      $��16(�5�      $	��Top=�5�      $��50��5�      $��Center=�5�      $��0q�5�      $��SemiBold"�5�      ��5�      �� *DeleteCardFieldCommand+��5�      �� $ ucField����5�      Z� ��5�      Z�  5�      Z� .��5�        $ DeleteOutline=�5�      $��34��5�      $��34��5�      5�      5�      5�      