import machine
import time
import socket


# D1 and D2 Pins
BUTTON_PINS = [5, 4]

LED = machine.Pin(2, machine.Pin.OUT)
LED.on()

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

def connect():
    s.connect(('ip-here', 6060))

def start():

    while True:
	message = s.recv(1024).decode('utf-8')
	print(message)

	if message == 'ON':
	    LED.off()

	elif message == 'OFF':
	    LED.on()
