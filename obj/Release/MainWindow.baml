   M S B A M L   `   `   `  ���� D  @RingClock, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RingClock	RingClock      RingClock.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     *customclr-namespace:RingClock     -     � Title$  
MainWindow��$	��432��$	��432�� � WindowStyle$
 None=����	#01000000$��0q� � AllowsTransparency .  � ShowInTaskbar .  &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=� � 	Resources    ellipseStyle �� 
TargetType �� Property �� Value  ButtonFocusVisual	 J� SnapsToDevicePixels!  System.Windows.Media.Colors
   SkyBlue �� StrokeDashArray  MetroButton �� 
TargetType ��Name �� Triggers P� Property P� Value��%  &	        &	 �     &	 �     7  �� " A�.���  � ��$ Center=���  � ��$ Center=���  � �� �	#55000000��  � ��$ 24����  �@J�SnapsToDevicePixels .  �� .���  � �� �� .R��� $��0q�	 .  #��� 
 $��1��$	 1 2R��� " ��.���  �@�FocusVisualStyle8   ��  � �� �	#EEEEEEEE��  � ��
 �������  � ��$ Center=���  � ��$ Center=���  � ��$ 0q���  � ��$ 14(���  � ��$ 0q���  � �� �� " ��.R�� .+��� $ Border��#��z��#��z��#��z���� #��z��#��z��#��z����.  P�  � �� . . ���  � ��$ 1q�P�  �@J�IsMouseOver . . ���  � ��$ 0q���  � ��
 ��·�P�  � {� .  . ���  � ��
 ��·�.���� -   $��canvas��.+�A� -   $��	bgEllipse��$	��432��$	��432�����	#55000000	 .  A� -   $��secondEllipse��$	��432��$	��432��
 � Tag$ Seconds��#��[  A� -   $��minuteEllipse��$	��374��$	��374��$ Minutes��#��[   �� Left$ 29��
 �� Top$ 29��A� -   $��hourEllipse��$	��316��$	��316��$ Hours��#��[  $ 58��$ 58��A� -   $��
dowEllipse��$	��258��$	��258��$ Day of week��#��[  $ 87��$ 87��A� -   $��
domEllipse��$	��200��$	��200��$ Day of month��#��[  $	 116��$	 116��A� -	   $��textEllipse�����	#55000000	 .  �� -
    ��Name$ timeText��
������ ���	#FFFFFFFF$��38��$��90��$
��Wrap=�$��Ubuntu)�$��00:00:00 00/00/0000�� �� TextAlignment$ Center=�$��16(�$	 172��$	 192���� -   $��btnExit��#��[ 
������ $��x��$	 416��$ 6��   RingClock.Arc  -     Name$ 	secondArc�����	#FFFFFFFF	 .     
StartAngle$ -1.57Q�   EndAngle$ -1.57Q�   
SmallAngle .    Radius$	 204Q�$	 216��$	 216��$��4��$ Seconds��  -   $ 	minuteArc�����	#FFFFFFFF	 .  $ -1.57Q�$ -1.57Q� . $	 175Q�$	 187��$	 187��$��4��$ Minutes��  -   $ hourArc�����	#FFFFFFFF	 .  $ -1.57Q�$ -1.57Q� . $	 146Q�$	 158��$	 158��$��4��$ Hours��  -   $ dowArc�����	#FFFFFFFF	 .  $ -1.57Q�$ -1.57Q� . $	 117Q�$	 129��$	 129��$��4��$ Day of week��  -   $ domArc�����	#FFFFFFFF	 .  $ -1.57Q�$ -1.57Q� . $ 88Q�$	 100��$	 100��$��4��$ Day of month��