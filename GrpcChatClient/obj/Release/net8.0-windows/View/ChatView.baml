   M S B A M L   `   `   `  ���� I  EGrpcChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:GrpcChatClient.ViewGrpcChatClient.View  "    GrpcChatClient.View.ChatView   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:GrpcChatClient.View       � 	Resources     NoMouseOverButtonTemplate �� 
TargetType �� Triggers P� Property P� Value �� Value �� Property��%�   &	        �� " ��.R��� #��z��#��z��#��z��._��� #��z��#��z�� P�  � {�$ False��. ��� # Z� � ���� # Z
� � ��.���  � ColumnDefinitions �� $��* ��� $��2* � � RowDefinitions �� $
��auto ��� .+��� $��0��$��0��$��2��	 �� Orientation$	 
Horizontal=�$��Center=�$��Center=�.+��� $&�� /images/icons/account_circle.png��$��30��$
��Left=�$��Center=��� -   
 ��Name$
 CurrentUsername��$��16(�$��	Open Sans)�$��Center=�$��Center=�$��Right=�$��15, 0, 0, 0q����� *
Username+�� $��0��$��1�� �� CornerRadius$ 5��$��10, 5, 5, 10q����#F8F8FF$��0q�._��  �� $
��auto ��� .+��� $��0��$	 
Horizontal=�$��Center=�$��
0, 5, 0, 5q�.+�� .+��� -    ��Name$ SearchInput��$��0q�
�������$��30��$	��200��$��Center=�$��9, 0, 10, 0q�$��
0, 0, 5, 0q����� *	AddRoom+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��.��   �� 
TargetType��%9   '��       �� " ��.���  �@��CornerRadius$ 5��.�� J� InputBindings �� 
 �� Key$ Return�� �� Command �� *JoinRoomCommand+.���� $ ��Add a room or someone.....�� J� IsHitTestVisible$ False��$��Center=�$
��Left=�
�������$��	Open Sans)�$��10, 0, 10, 0q����� " ��.���  � y�$ 	Collapsed=� �� Triggers ��  �� Binding �� *Text+ �� ElementName$ SearchInput�� �� Value  .a���  � y�$ Visible=��� -    ��Name$ AddRoomButton��$��
5, 0, 0, 0q�$��75��$��30�����#40404F$��0q�#��[  ���� *JoinRoomCommand+.���� $!��/images/icons/add_white.png��$��Center=�$��Center=�$��25��  ��%9   '��       �� " ��.���  �@��CornerRadius$ 5����   RoomItemTemplate#��[ $��1��$��
0, 5, 0, 0q�
������ $��0q����� *Rooms+ �� SelectedItem �� *SelectedRoom+�� $��1��$��1��$ 5��$��5, 5, 10, 10q����#F8F8FF._��  �� �� $
��auto �.+��� $��0��$��0, 10, 0, 10q�
������ $��0q�  ChatItemTemplate#��[ ���� *SelectedRoom.Messages+���� .R�$� $��Bottom=��� $��1��$	 
Horizontal=�.+�� $��Center=�$
��Left=�$��20, 5, 0, 5q�.+��� -   $ MessageInput��$��40��$	��540��$��0q����#EDF1FE$��Center=����#40404F$��14(�$��	Open Sans)�$��19, 0, 20, 0q����� *	Message+$ TwoWay=�$ PropertyChanged=�.��.��  ��%:   '��       �� " ��.���  �@��CornerRadius$ 20��.�� �� $ Return�� �� *SendCommand+.���� $��Message.....��$ False��$��Center=�$
��Left=�
�������$��14(�$��	Open Sans)�$��20, 0, 20, 0q����� " ��.���  � y�$ 	Collapsed=� ��  �� *Text+$ MessageInput��  .a���  � y�$ Visible=��� $��40��$��40��$��Center=�$��Right=�$��15, 0, 0, 0q�$��0q����#40404F#��[  ���� *SendCommand+.���� $��/images/icons/send.png��$��Center=�$��Center=�$��25��  ��%:   '��       �� " ��.���  �@��CornerRadius$ 50��