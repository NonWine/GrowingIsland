// read_files.js
// Usage:
//   deno run --allow-read read_files.js [path]
// Example:
//   deno run --allow-read read_files.js Assets
//
// This script lists entries in the given directory (or current directory by default),
// printing name, type (file/dir/other) and size for files.

const target = Deno.args[0] ?? ".";

for await (const entry of Deno.readDir(target)) {
  const type = entry.isDirectory ? "dir" : entry.isFile ? "file" : "other";
  let sizeInfo = "";
  if (entry.isFile) {
    try {
      const st = await Deno.stat(`${target}/${entry.name}`);
      sizeInfo = ` - ${st.size} bytes`;
    } catch (err) {
      // ignore stat errors (permissions, race conditions, etc.)
      sizeInfo = " - size unknown";
    }
  }
  console.log(`${entry.name} [${type}]${sizeInfo}`);
}
