var semver = require('semver');

module.exports = function(grunt) {
  var pkg = grunt.file.readJSON('package.json');
  pkg.version = semver.clean(pkg.version);
  pkg.majorMinorPatch = semver.major(pkg.version) + '.' + semver.minor(pkg.version) + '.' + semver.patch(pkg.version);

  grunt.initConfig({
    pkg: pkg,

    bump: {
      options: {
        files: ['package.json'],
        commit: false,
        createTag: false,
        push: false,
        prereleaseName: 'rc'
      }
    },

    clean: {
      build: ['build', 'src/**/bin', 'src/**/obj'],
      nuget: ['src/packages'],
      pack: ["*.nupkg"]
    },

    "string-replace": {
      version: {
        options: {
          replacements: [{
            pattern: /\[assembly: AssemblyVersion\(".*"\)\]/g,
            replacement: '[assembly: AssemblyVersion("<%= pkg.majorMinorPatch %>")]'
          }, {
            pattern: /\[assembly: AssemblyFileVersion\(".*"\)\]/g,
            replacement: '[assembly: AssemblyFileVersion("<%= pkg.majorMinorPatch %>")]'
          }, {
            pattern: /\[assembly: AssemblyInformationalVersion\(".*"\)\]/g,
            replacement: '[assembly: AssemblyInformationalVersion("<%= pkg.version %>")]'
          }]
        },
        files: [{'src/': 'src/**/Properties/AssemblyInfo.cs'}]
      }
    },

    nugetrestore: {
      restore: {
        src: 'src/**/packages.config',
        dest: 'src/packages'
      }
    },

    msbuild: {
      options: {
        version: 4.0,
        targets: ['Clean', 'Rebuild'],
        stdout: true,
        maxCpuCount: 4,
        buildParameters: {
          WarningLevel: 2
        },
        verbosity: 'quiet'
      },
      dev: {
        src: ['src/**/*.sln'],
        options: {
          projectConfiguration: 'Debug'
        }
      },
      rel: {
        src: ['src/**/*.sln'],
        options: {
          projectConfiguration: 'Release'
        }
      }
    },

    copy: {
      cliDev: {
        expand: true, cwd: 'src/AtlasTexturePacker/bin/Debug', src: '**', dest: 'build/CLI/'
      },
      libDev: {
        expand: true, cwd: 'src/AtlasTexturePackerLib/bin/Debug', src: '**', dest: 'build/lib/'
      },
      cliRel: {
        expand: true, cwd: 'src/AtlasTexturePacker/bin/Release', src: ['**', '!**/*.pdb'], dest: 'build/CLI/'
      },
      libRel: {
        expand: true, cwd: 'src/AtlasTexturePackerLib/bin/Release', src: ['**', '!**/*.pdb'], dest: 'build/lib/'
      }
    },

    //FIXME: until Nuget 3.0 is released and can be used, semver 2.0 prerelease strings are not fully supported
    nugetpack: {
      lib: {
        options: { version: pkg.version },
        src: 'libPackage.nuspec',
        dest: './'
      },
	  cli: {
		  options:{ version: pkg.version},
		  src: 'cliPackage.nuspec',
		  dest: './'
	  }
	  
    }
  });

  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-copy');
  grunt.loadNpmTasks('grunt-string-replace');
  grunt.loadNpmTasks('grunt-nuget');
  grunt.loadNpmTasks('grunt-msbuild');
  grunt.loadNpmTasks('grunt-bump');


  grunt.registerTask('clean:all', ['clean:build', 'clean:nuget', 'clean:pack']);
  grunt.registerTask('set:version', ['string-replace:version']);
  grunt.registerTask('build:dev', ['clean:all', 'set:version', 'nugetrestore', 'msbuild:dev', 'copy:cliDev', 'copy:libDev']);
  grunt.registerTask('build:rel', ['clean:all', 'set:version', 'nugetrestore', 'msbuild:rel', 'copy:cliRel', 'copy:libRel']);
  grunt.registerTask('build', ['build:dev']);
  grunt.registerTask('pack:dev', ['clean:pack', 'nugetpack:lib', 'nugetpack:cli']);
  grunt.registerTask('pack:rel', ['clean:pack', 'nugetpack:lib', 'nugetpack:cli']);
  grunt.registerTask('pack', ['pack:dev']);
  grunt.registerTask('dev', ['build:dev', 'pack:dev']);
  grunt.registerTask('rel', ['build:rel', 'pack:rel']);
  grunt.registerTask('default', ['dev']);
  grunt.registerTask('relnnr', ['clean:all', 'set:version', 'msbuild:rel', 'copy:cliRel', 'copy:libRel']);
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
};
