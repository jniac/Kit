let fileTemplate = `
using System;

namespace Kit.CoreV1
{
	public partial class Event
	{
$
	}
}
`.trim()


let methodTemplate = `

// $info
public static Listener On<T>(object target, object type,
	$T1 callback = null,
	$T2 enter = null,
	$T3 exit = null,
	object key = null)
	where T : Event
	=> On<T>(target, type, ToAction<T>(callback), ToAction<T>(enter), ToAction<T>(exit), key);

// $info
public static Listener On<T>(object target,
	$T1 callback = null,
	$T2 enter = null,
	$T3 exit = null,
	object key = null)
	where T : Event
	=> On<T>(target, "*", ToAction<T>(callback), ToAction<T>(enter), ToAction<T>(exit), key);

// $info
public static Listener On<T>(
	$T1 callback = null,
	$T2 enter = null,
	$T3 exit = null,
	object key = null)
	where T : Event
	=> On<T>(global, "*", ToAction<T>(callback), ToAction<T>(enter), ToAction<T>(exit), key);

`.trim()

let types = [
	{
		comm: 'basic',
		type: generic => generic ? 'Action<T>' : 'Action<Event>',
		basic: true,
	},
	{
		comm: 'no-args',
		type: generic => 'Action',
	},
	{
		comm: 'async',
		type: generic => generic ? 'Func<T, Task>' : 'Func<Event, Task>',
	},
	{
		comm: 'async-no-args',
		type: generic => 'Func<Task>',
	},
]

// combination:
// 100, 200, 300
// 010, 011, 001
// 020, 022, 002
// 021, 012

// 00, 01, 10, 11

// 00, 01, 02, 03,
// 10, 11, 12, 13
// 20, 21, 22, 23
// 30, 31, 32, 33
