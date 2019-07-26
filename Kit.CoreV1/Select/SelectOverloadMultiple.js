#!/usr/bin/env node

/*

usage:

node ./SelectOverloadMultiple.js

*/

const fs = require('fs')

let fileTemplate = `

namespace Kit.CoreV1
{
	// generated with SelectOverloadMultiple.js
    public partial class Select<T>
    {
        $enter

        $exit
    }
}

`.trim()

let outputFilename = 'SelectOverloadMultiple.cs'
let max = 4

const prependTabs = (str, n, tab = '    ') =>
    str.split('\n').map(s => tab.repeat(n) + s).join('\n')

function* range(max) {
    let i = 0
    while(i < max)
        yield i++
}

const getMethod = (name, n) => {

    let genericArgs = [...range(n)].map(i => `TLayer${i + 1}`).join(', ')
    let lines = [...range(n)].map(i => `GetLayer<TLayer${i + 1}>().${name}(item);`).join('\n')

    return prependTabs(`public void ${name}<${genericArgs}>(T item) \n{\n${prependTabs(lines, 1)}\n}`, 2)

}

let script = fileTemplate
	.replace(/[ \t]*\$enter/g, [...range(max - 1)].map(i => getMethod('Enter', i + 2)).join('\n'))
	.replace(/[ \t]*\$exit/g, [...range(max - 1)].map(i => getMethod('Exit', i + 2)).join('\n'))



fs.writeFile(`./${outputFilename}`, script, (error) => {

    if (error)
        return console.log(error)

    console.log(`The file (${outputFilename}) was saved!`)

})
