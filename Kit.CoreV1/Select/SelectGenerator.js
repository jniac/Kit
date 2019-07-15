#!/usr/bin/env node

/*

	Finally not used!!

*/

const fs = require('fs')

let outputFilename = 'GenericSelect.cs'
let maxGenericCount = 8

let fileTemplate = `

using System;

namespace Kit.CoreV1
{
$
}

`.trim()

let constructorTemplate = `

	public class Select<T, $1> : Select<T>
	{
		public Select() : base($2) { }
	}

`.trim()

function* range(max) {

    let i = 0

	while(i < max)
        yield i++

}

let replacer = {

	$1: (n) => [...range(n)].map(i => `TLayer${i + 1}`).join(', '),
	$2: (n) => [...range(n)].map(i => `typeof(TLayer${i + 1})`).join(', '),

}

let blocks = [...range(maxGenericCount)].map(i =>
	'\t' + constructorTemplate.replace(/\$\d/g, m => replacer[m](i + 1)))

let script = fileTemplate.replace(/\$/g, blocks.join('\n\n'))

fs.writeFile(`./${outputFilename}`, script, (error) => {

    if (error)
        return console.log(error)

    console.log(`The file (${outputFilename}) was saved!`)

})
