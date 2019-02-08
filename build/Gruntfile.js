module.exports = function(grunt) {

    var path = require('path');

    // Load the package JSON file
    var pkg = grunt.file.readJSON('package.json');

    // Get the project paths
    var sourceDir = '../src/',
        projectDir = sourceDir + pkg.name + '/';

	// get the root path of the project
    var projectRoot = '../src/' + pkg.name + '/';
	
	// get the release paths
    var releaseDir = 'releases/',
        releaseFilesDir = releaseDir + 'files/';

    // Load information about the assembly
    //var assembly = grunt.file.readJSON(projectRoot + 'Properties/AssemblyInfo.json');

    // Get the version of the package
    //var version = assembly.informationalVersion ? assembly.informationalVersion : assembly.version;

    grunt.initConfig({
        pkg: pkg,
        //pkgMeta: pkgMeta,
        clean: {
            files: [
                releaseFilesDir + '**/*.*' //'releases/temp/'
            ]
        },
        copy: {
            /*release: {
                files: [
                    {
                        expand: true,
                        cwd: projectRoot + 'bin/Release/',
                        src: [
                            pkgMeta.name + '.dll',
                            pkgMeta.name + '.xml'
                        ],
                        dest: 'files/bin/'
                    }
                ]
            }*/
            bacon: {
                files: [
                    {
                        expand: true,
                        cwd: sourceDir + pkg.name + '/' + 'obj/Release/', //projectDir + 'obj/Release/',
                        src: [
                            pkg.name + '.dll',
                            pkg.name + '.xml'
                        ],
                        dest: releaseFilesDir + 'bin/'
                    },
                    {
                        expand: true,
                        cwd: sourceDir + pkg.name + '.Web/App_Plugins/' + pkg.name, //projectDir + '.Web/',
                        src: ['**'],
                        dest: releaseFilesDir
                    }
                ]
            }
        },
		zip: {
            release: {
                cwd: releaseFilesDir,
                src: [
                    releaseFilesDir + '**/*.*'
                ],
                dest: releaseDir + 'zip/' + pkg.name + '.v' + pkg.version + '.zip'
            }
        },
        umbracoPackage: {
            release: {
                src: releaseFilesDir,
                dest: releaseDir + '/umbraco',
                options: {
                    name: pkg.name,
                    version: pkg.version,
                    url: pkg.url,
                    license: pkg.license,
                    licenseUrl: "https://opensource.org/licenses/MIT",
                    author: pkg.author.name,
                    authorUrl: pkg.author.url,
                    readme: pkg.readme,
                    outputName: pkg.name + '.v' + pkg.version + '.zip'
                }
            }
        },
		nugetpack: {
            release: {
                src: projectDir + pkg.name + '.csproj',
                //src: '../src/package.nuspec', //'src/' + pkg.name + '/' + pkg.name + '.csproj',
                dest: releaseDir + '/nuget/',
                options: {
                    properties: 'Platform=AnyCPU;Configuration=Release'
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-umbraco-package');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-nuget');
    grunt.loadNpmTasks('grunt-zip');

    grunt.registerTask('dev', ['clean', 'copy', 'nugetpack', 'zip', 'umbracoPackage']);
    grunt.registerTask('default', ['dev']);
};